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
                
                // Register Arabic font
                var fontPath = @"C:\Fonts\Amiri-Regular.ttf";
                if (!System.IO.File.Exists(fontPath))
                {
                    throw new FileNotFoundException($"Font file not found at {fontPath}");
                }
                FontFactory.Register(fontPath, "Amiri");
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

            // Build the attendance report using Baghdad's current date
            var dailyAttendance = allOffices
                .Select(office => new
                {
                    GovernorateName = office.Governorate?.Name ?? "_",
                    OfficeName = office.Name ?? "-",
                    OfficeCode = office.Code,
                    TodayAttendance = attendances
                        .Where(a => a.Date.Date == todayBaghdad &&
                                    a.Office?.Code == office.Code)
                        .ToList()
                })
                .Select(office => new
                {
                    GovernorateName = office.GovernorateName,
                    OfficeName = office.OfficeName,
                    OfficeCode = office.OfficeCode,
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
            var titleFont = FontFactory.GetFont("Amiri", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 24);
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

            // Report Date Table using Baghdad date
            var dateFont = FontFactory.GetFont("Amiri", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 12);
            var dateCell = new PdfPCell(new Phrase(ShapeArabicText($"تاريخ التقرير: {todayBaghdad:yyyy-MM-dd}"), dateFont))
            {
                BackgroundColor = BaseColor.WHITE,
                HorizontalAlignment = Element.ALIGN_RIGHT,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                Padding = 10f,
                Border = iTextRectangle.NO_BORDER,
                RunDirection = PdfWriter.RUN_DIRECTION_RTL
            };

            var dateTable = new PdfPTable(1)
            {
                WidthPercentage = 100,
                SpacingAfter = 10f
            };
            dateTable.AddCell(dateCell);
            document.Add(dateTable);

            // Main Data Table
            var table = new PdfPTable(5)
            {
                WidthPercentage = 100,
                SpacingBefore = 10f,
                SpacingAfter = 20f,
                RunDirection = PdfWriter.RUN_DIRECTION_RTL
            };

            float[] columnWidths = new float[] { 20f, 25f, 15f, 15f, 25f };
            table.SetWidths(columnWidths);

            // Updated: Align header text to center
            AddTableHeader(table, new[]
            {
                ShapeArabicText("اسم المحافظة"),
                ShapeArabicText("اسم المكتب"),
                ShapeArabicText("الحضور الصباحي"),
                ShapeArabicText("الحضور المسائي"),
                ShapeArabicText("إجمالي الحضور")
            });

            // Add a row for each office
            foreach (var entry in dailyAttendance)
            {
                // Choose a red background if there is no attendance for the office
                var backgroundColor = entry.TotalAttendance == 0 ? NO_ATTENDANCE_ROW_COLOR : BaseColor.WHITE;

                // Updated: Align row text to center
                AddTableRow(table, new[]
                {
                    ShapeArabicText(entry.GovernorateName),
                    ShapeArabicText(entry.OfficeName),
                    entry.MorningAttendance.ToString(),
                    entry.EveningAttendance.ToString(),
                    entry.TotalAttendance.ToString()
                }, backgroundColor);
            }

            document.Add(table);

            // Footer Table
            var footerFont = FontFactory.GetFont("Amiri", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 8);
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

        /// <summary>
        /// Adds a table row with an optional background color.
        /// </summary>
        /// <param name="table">The PdfPTable to add the row to.</param>
        /// <param name="cells">The string values for each cell in the row.</param>
        /// <param name="backgroundColor">
        /// Optional background color for the row (default is white).
        /// Use a red color for rows with no attendances.
        /// </param>
        private void AddTableRow(PdfPTable table, string[] cells, BaseColor backgroundColor = null)
        {
            backgroundColor ??= BaseColor.WHITE;
            var cellFont = FontFactory.GetFont("Amiri", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 10);

            foreach (string cellContent in cells)
            {
                var cell = new PdfPCell(new Phrase(ShapeArabicText(cellContent ?? "-"), cellFont))
                {
                    BackgroundColor = backgroundColor,
                    // Changed from ALIGN_RIGHT to ALIGN_CENTER
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

        /// <summary>
        /// Adds the table header cells.
        /// </summary>
        /// <param name="table">The PdfPTable to add the headers to.</param>
        /// <param name="headers">An array of header text values.</param>
        private void AddTableHeader(PdfPTable table, string[] headers)
        {
            var headerFont = FontFactory.GetFont("Amiri", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 12);

            foreach (string header in headers)
            {
                var cell = new PdfPCell(new Phrase(ShapeArabicText(header), headerFont))
                {
                    BackgroundColor = TABLE_HEADER_COLOR,
                    // Changed from ALIGN_RIGHT to ALIGN_CENTER
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

        /// <summary>
        /// A placeholder method for Arabic text shaping.
        /// Replace with proper Arabic shaping logic if needed.
        /// </summary>
        /// <param name="text">The text to shape.</param>
        /// <returns>The shaped text.</returns>
        private string ShapeArabicText(string text)
        {
            return text;
        }
    }
}
