using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.EntityFrameworkCore;
using OMSV1.Domain.Entities.DamagedPassport;
using OMSV1.Domain.Entities.Offices;
using OMSV1.Domain.Interfaces;
using OMSV1.Infrastructure.Interfaces;
using OMSV1.Infrastructure.Persistence;
using iTextRectangle = iTextSharp.text.Rectangle;

namespace OMSV1.Infrastructure.Services
{
    public class DamagedPassportPdfService : IDamagedPassportService
    {
        private readonly AppDbContext _context;
        private static readonly BaseColor TABLE_HEADER_COLOR = new BaseColor(240, 240, 240);
        private static readonly BaseColor BORDER_COLOR = new BaseColor(120, 120, 120);

        public DamagedPassportPdfService(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        static DamagedPassportPdfService()
        {
            try
            {
                // Register additional encoding providers
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
                throw new InvalidOperationException("Error during static initialization of DamagedPassportPdfService.", ex);
            }
        }

        public async Task<byte[]> GenerateDailyDamagedPassportsPdfAsync(List<DamagedPassport> damagedPassports)
        {
            if (damagedPassports == null)
                throw new ArgumentNullException(nameof(damagedPassports));

            // Define the UTC boundaries for today
            var startOfTodayUtc = DateTime.UtcNow.Date;
            var endOfTodayUtc = startOfTodayUtc.AddDays(1).AddTicks(-1);

            // Filter the provided list to get only the passports created today (in UTC)
            var dailyDamagedPassports = damagedPassports
                .Where(dp => dp.DateCreated >= startOfTodayUtc && dp.DateCreated <= endOfTodayUtc)
                .ToList();

            // Fetch all offices along with their Governorate in a single query
            var allOffices = await _context.Offices
                .Include(o => o.Governorate)
                .ToListAsync();

            if (!allOffices.Any())
                throw new InvalidOperationException("No offices found in the system.");

            // For each office, count the number of damaged passports registered today.
            // This is a left join: if no damaged passports are found for an office, the count is zero.
            var officesWithDamagedPassports = allOffices
                .Select(office => new
                {
                    GovernorateName = office.Governorate?.Name ?? "-",
                    OfficeName = office.Name ?? "-",
                    OfficeCode = office.Code,  // Include OfficeCode here
                    DamagedPassportCount = dailyDamagedPassports
                        .Where(dp => dp.Office?.Code == office.Code)
                        .Count()
                })
                .OrderBy(x => x.OfficeCode) // Order by OfficeCode
                .ToList();

            // Begin PDF generation
            using var memoryStream = new MemoryStream();
            using var document = new Document(PageSize.A4, 50, 50, 50, 50);
            PdfWriter.GetInstance(document, memoryStream);

            document.Open();

            // Title Table
            var titleFont = FontFactory.GetFont("Amiri", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 24);
            var titleCell = new PdfPCell(new Phrase(ShapeArabicText("تقرير الجوازات التالفة اليومية"), titleFont))
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

            // Report Date Table
            var dateFont = FontFactory.GetFont("Amiri", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 12);
            var dateCell = new PdfPCell(new Phrase(ShapeArabicText($"تاريخ التقرير: {DateTime.UtcNow:yyyy-MM-dd}"), dateFont))
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
            // The column order is:
            // 1. اسم المحافظة (Governorate Name)
            // 2. اسم المكتب (Office Name)
            // 3. عدد الجوازات التالفة (Damaged Passport Count)
            var table = new PdfPTable(3)
            {
                WidthPercentage = 100,
                SpacingBefore = 10f,
                SpacingAfter = 20f,
                RunDirection = PdfWriter.RUN_DIRECTION_RTL
            };

            float[] columnWidths = new float[] { 40f, 30f, 30f }; // Adjust widths if needed
            table.SetWidths(columnWidths);

            AddTableHeader(table, new[]
            {
                ShapeArabicText("اسم المحافظة"),
                ShapeArabicText("اسم المكتب"),
                ShapeArabicText("عدد الجوازات التالفة")
            });

            foreach (var entry in officesWithDamagedPassports)
            {
                AddTableRow(table, new[]
                {
                    ShapeArabicText(entry.GovernorateName),
                    ShapeArabicText(entry.OfficeName),
                    entry.DamagedPassportCount.ToString()
                });
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

        private void AddTableRow(PdfPTable table, string[] cells)
        {
            if (table == null) throw new ArgumentNullException(nameof(table));
            if (cells == null) throw new ArgumentNullException(nameof(cells));

            var cellFont = FontFactory.GetFont("Amiri", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 10);

            foreach (string cellContent in cells)
            {
                var cell = new PdfPCell(new Phrase(ShapeArabicText(cellContent ?? "-"), cellFont))
                {
                    HorizontalAlignment = Element.ALIGN_RIGHT,
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
            var headerFont = FontFactory.GetFont("Amiri", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 12);

            foreach (string header in headers)
            {
                var cell = new PdfPCell(new Phrase(ShapeArabicText(header), headerFont))
                {
                    BackgroundColor = TABLE_HEADER_COLOR,
                    HorizontalAlignment = Element.ALIGN_RIGHT,
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
            // Placeholder for Arabic text shaping logic.
            return text; // Replace with a proper shaping implementation if needed.
        }
    }
}
