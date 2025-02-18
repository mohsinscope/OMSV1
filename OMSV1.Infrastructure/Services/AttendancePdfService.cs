using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.EntityFrameworkCore;
using OMSV1.Domain.Entities.Attendances;
using OMSV1.Domain.Entities.Offices;
using OMSV1.Domain.Enums;
using OMSV1.Domain.Interfaces;
using OMSV1.Domain.SeedWork;
using OMSV1.Infrastructure.Interfaces;
using OMSV1.Infrastructure.Persistence;
using iTextRectangle = iTextSharp.text.Rectangle;

namespace OMSV1.Infrastructure.Services
{
    public class AttendancePdfService : IAttendanceService
    {
        private readonly IGenericRepository<Office> _officeRepository;
        private readonly AppDbContext _context; // Inject the DbContext

        // Existing colors for header and border
        private static readonly BaseColor TABLE_HEADER_COLOR = new BaseColor(240, 240, 240);
        private static readonly BaseColor BORDER_COLOR = new BaseColor(120, 120, 120);
        // New color for rows with no attendances (light red)
        private static readonly BaseColor NO_ATTENDANCE_ROW_COLOR = new BaseColor(255, 200, 200);

        public AttendancePdfService(
            IGenericRepository<Office> officeRepository,
            AppDbContext context) // Add DbContext parameter here
        {
            _officeRepository = officeRepository ?? throw new ArgumentNullException(nameof(officeRepository));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        
        static AttendancePdfService()
        {
            try
            {
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                
                // If needed, you can register the Times New Roman font.
                // For example:
                FontFactory.Register(@"C:\Windows\Fonts\times.ttf", "Times New Roman");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error during static initialization of AttendancePdfService.", ex);
            }
        }

        public async Task<byte[]> GenerateDailyAttendancePdfAsync(List<Attendance> attendances)
        {
            if (attendances == null)
                throw new ArgumentNullException(nameof(attendances));

            // Configure current time for Baghdad (UTC+3)
            var baghdadNow = DateTime.UtcNow.AddHours(3);
            var todayBaghdad = baghdadNow.Date.AddDays(-1);

            // Fetch all offices along with their Governorate in a single query
            var allOffices = await _context.Offices
                .Include(o => o.Governorate)
                .ToListAsync();

            if (!allOffices.Any())
                throw new InvalidOperationException("No offices found in the system.");

            // Build the attendance report using Baghdad's current date.
            // Compute TotalStaff as the sum of the staff properties.
            var dailyAttendance = allOffices
                .Select(office => new
                {
                    GovernorateName = office.Governorate?.Name ?? "_",
                    OfficeName = office.Name ?? "-",
                    OfficeCode = office.Code,
                    TotalStaff = office.ReceivingStaff + office.AccountStaff + office.PrintingStaff + office.QualityStaff + office.DeliveryStaff,
                    TodayAttendance = attendances
                        .Where(a => a.Date.Date == todayBaghdad &&
                                    a.Office?.Code == office.Code)
                        .ToList()
                })
                .Select(office => new
                {
                    office.GovernorateName,
                    office.OfficeName,
                    office.OfficeCode,
                    office.TotalStaff,
                    MorningAttendance = office.TodayAttendance
                        .Where(a => a.WorkingHours.HasFlag(WorkingHours.Morning))
                        .Sum(a => a.ReceivingStaff + a.AccountStaff + a.PrintingStaff + a.QualityStaff + a.DeliveryStaff),
                    EveningAttendance = office.TodayAttendance
                        .Where(a => a.WorkingHours.HasFlag(WorkingHours.Evening))
                        .Sum(a => a.ReceivingStaff + a.AccountStaff + a.PrintingStaff + a.QualityStaff + a.DeliveryStaff),
                    TotalAttendance = office.TodayAttendance
                        .Sum(a => a.ReceivingStaff + a.AccountStaff + a.PrintingStaff + a.QualityStaff + a.DeliveryStaff)
                })
                .OrderBy(a => a.OfficeCode)
                .ToList();

            // PDF Generation Logic
            using var memoryStream = new MemoryStream();
            using var document = new Document(PageSize.A4, 50, 50, 50, 50);
            var writer = PdfWriter.GetInstance(document, memoryStream);

            document.Open();

            // Title Table
            var titleFont = FontFactory.GetFont("Times New Roman", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 24, Font.NORMAL);
            var titleCell = new PdfPCell(new Phrase(ShapeArabicText("تقرير الحضور اليومي"), titleFont))
            {
                BackgroundColor = BaseColor.WHITE,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                Padding = 10f,
                Border = iTextRectangle.NO_BORDER,
                RunDirection = PdfWriter.RUN_DIRECTION_RTL
            };

            var titleTable = new PdfPTable(1)
            {
                WidthPercentage = 100,
                SpacingAfter = 20f
            };
            titleTable.AddCell(titleCell);
            document.Add(titleTable);

            // Report Date Table using Baghdad date, aligned to right
            var dateFont = FontFactory.GetFont("Times New Roman", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 12, Font.NORMAL);
            var dateParagraph = new Paragraph(ShapeArabicText($"تاريخ التقرير: {todayBaghdad:yyyy-MM-dd}"), dateFont)
            {
                Alignment = Element.ALIGN_RIGHT
            };

            var dateCell = new PdfPCell(dateParagraph)
            {
                BackgroundColor = BaseColor.WHITE,
                Padding = 10f,
                Border = iTextRectangle.NO_BORDER,
                RunDirection = PdfWriter.RUN_DIRECTION_RTL
            };

            var dateTable = new PdfPTable(1)
            {
                WidthPercentage = 100,
                SpacingAfter = 10f,
                RunDirection = PdfWriter.RUN_DIRECTION_RTL
            };
            dateTable.AddCell(dateCell);
            document.Add(dateTable);

            // Main Data Table with an additional تسلسل column.
            // Column order:
            // 1. تسلسل (Sequence)
            // 2. اسم المحافظة (Governorate Name)
            // 3. اسم المكتب (Office Name)
            // 4. الحضور الصباحي (Morning Attendance / Total Staff)
            // 5. الحضور المسائي (Evening Attendance / Total Staff)
            // 6. إجمالي الحضور (Total Attendance)
            var table = new PdfPTable(6)
            {
                WidthPercentage = 100,
                SpacingBefore = 10f,
                SpacingAfter = 20f,
                RunDirection = PdfWriter.RUN_DIRECTION_RTL
            };

            float[] columnWidths = new float[] { 12f, 20f, 25f, 15f, 15f, 25f };
            table.SetWidths(columnWidths);

            AddTableHeader(table, new[]
            {
                ShapeArabicText("تسلسل"),
                ShapeArabicText("اسم المحافظة"),
                ShapeArabicText("اسم المكتب"),
                ShapeArabicText("الحضور الصباحي"),
                ShapeArabicText("الحضور المسائي"),
                ShapeArabicText("إجمالي الحضور")
            });

            int sequence = 1;
            foreach (var entry in dailyAttendance)
            {
                var backgroundColor = entry.TotalAttendance == 0 ? NO_ATTENDANCE_ROW_COLOR : BaseColor.WHITE;

                AddTableRow(table, new[]
                {
                    sequence.ToString(),
                    ShapeArabicText(entry.GovernorateName),
                    ShapeArabicText(entry.OfficeName),
                    $"{entry.MorningAttendance}/{entry.TotalStaff}",
                    $"{entry.EveningAttendance}/{entry.TotalStaff}",
                    entry.TotalAttendance.ToString()
                }, backgroundColor);

                sequence++;
            }

            document.Add(table);

            // Overall Total Block
            var overallTotal = dailyAttendance.Sum(x => x.TotalAttendance);
            var overallTotalFont = FontFactory.GetFont("Times New Roman", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 14, Font.BOLD);
            var overallTotalCell = new PdfPCell(new Phrase(ShapeArabicText("اجمالي الحضور: " + overallTotal), overallTotalFont))
            {
                BackgroundColor = BaseColor.WHITE,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                Padding = 10f,
                Border = iTextRectangle.NO_BORDER,
                RunDirection = PdfWriter.RUN_DIRECTION_RTL
            };

            var overallTotalTable = new PdfPTable(1)
            {
                WidthPercentage = 100,
                SpacingBefore = 10f,
                SpacingAfter = 20f
            };
            overallTotalTable.AddCell(overallTotalCell);
            document.Add(overallTotalTable);

            // Footer Table
            var footerFont = FontFactory.GetFont("Times New Roman", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 8, Font.NORMAL);
            var footerCell = new PdfPCell(new Phrase(ShapeArabicText("تم إنشاؤه بواسطة OMS"), footerFont))
            {
                BackgroundColor = BaseColor.WHITE,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                Padding = 10f,
                Border = iTextRectangle.NO_BORDER,
                RunDirection = PdfWriter.RUN_DIRECTION_RTL
            };

            var footerTable = new PdfPTable(1)
            {
                WidthPercentage = 100
            };
            footerTable.AddCell(footerCell);
            document.Add(footerTable);

            document.Close();

            return await Task.FromResult(memoryStream.ToArray());
        }

        private void AddTableRow(PdfPTable table, string[] cells, BaseColor backgroundColor = null)
        {
            backgroundColor ??= BaseColor.WHITE;
            var cellFont = FontFactory.GetFont("Times New Roman", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 10, Font.NORMAL);

            foreach (string cellContent in cells)
            {
                var cell = new PdfPCell(new Phrase(ShapeArabicText(cellContent ?? "-"), cellFont))
                {
                    BackgroundColor = backgroundColor,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    Padding = 6f,
                    BorderColor = BORDER_COLOR,
                    MinimumHeight = 20f,
                    RunDirection = PdfWriter.RUN_DIRECTION_RTL
                };
                table.AddCell(cell);
            }
        }

        private void AddTableHeader(PdfPTable table, string[] headers)
        {
            var headerFont = FontFactory.GetFont("Times New Roman", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 12, Font.BOLD);

            foreach (string header in headers)
            {
                var cell = new PdfPCell(new Phrase(ShapeArabicText(header), headerFont))
                {
                    BackgroundColor = TABLE_HEADER_COLOR,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    Padding = 8f,
                    BorderColor = BORDER_COLOR,
                    MinimumHeight = 25f,
                    RunDirection = PdfWriter.RUN_DIRECTION_RTL
                };
                table.AddCell(cell);
            }
        }

        private string ShapeArabicText(string text)
        {
            return text;
        }
    }
}
