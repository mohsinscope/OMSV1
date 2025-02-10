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
        // Red highlight for offices with no damaged passports.
        private static readonly BaseColor NO_DAMAGED_PASSPORT_ROW_COLOR = new BaseColor(255, 200, 200);
        // Green highlight for the total summary box.
        private static readonly BaseColor TOTAL_BOX_COLOR = new BaseColor(200, 255, 200);

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

            // Configure Baghdad time (UTC+3)
            var baghdadNow = DateTime.UtcNow.AddHours(3);
            // Adjust to use the report date (yesterday) instead of today:
            var reportDate = baghdadNow.Date.AddDays(-1);
            var startOfReportDay = reportDate;
            var endOfReportDay = reportDate.AddDays(1).AddTicks(-1);

            // Filter the provided list to get only the passports created on the report date (in Baghdad time)
            var dailyDamagedPassports = damagedPassports
                .Where(dp => dp.DateCreated >= startOfReportDay && dp.DateCreated <= endOfReportDay)
                .ToList();

            // Fetch all offices along with their Governorate in a single query
            var allOffices = await _context.Offices
                .Include(o => o.Governorate)
                .ToListAsync();

            if (!allOffices.Any())
                throw new InvalidOperationException("No offices found in the system.");

            // For each office, count the number of damaged passports registered on the report date.
            // This is a left join: if no damaged passports are found for an office, the count is zero.
            var officesWithDamagedPassports = allOffices
                .Select(office => new
                {
                    GovernorateName = office.Governorate?.Name ?? "-",
                    OfficeName = office.Name ?? "-",
                    OfficeCode = office.Code,
                    DamagedPassportCount = dailyDamagedPassports
                        .Where(dp => dp.Office?.Code == office.Code)
                        .Count()
                })
                .OrderBy(x => x.OfficeCode)
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

            // Report Date Table using the report date (yesterday)
            var dateFont = FontFactory.GetFont("Amiri", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 12);
            var dateCell = new PdfPCell(new Phrase(ShapeArabicText($"تاريخ التقرير: {reportDate:yyyy-MM-dd}"), dateFont))
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

            // Build the Main Data Table
            // Column order:
            // 1. اسم المحافظة (Governorate Name)
            // 2. اسم المكتب (Office Name)
            // 3. عدد الجوازات التالفة (Damaged Passport Count)
            var mainTable = new PdfPTable(3)
            {
                WidthPercentage = 100,
                SpacingBefore = 10f,
                SpacingAfter = 10f,
                RunDirection = PdfWriter.RUN_DIRECTION_RTL
            };

            float[] columnWidths = new float[] { 40f, 30f, 30f }; // Adjust widths if needed
            mainTable.SetWidths(columnWidths);

            AddTableHeader(mainTable, new[]
            {
                ShapeArabicText("اسم المحافظة"),
                ShapeArabicText("اسم المكتب"),
                ShapeArabicText("عدد الجوازات التالفة")
            });

            foreach (var entry in officesWithDamagedPassports)
            {
                // Set red highlight if no damaged passports exist for the office
                var backgroundColor = entry.DamagedPassportCount == 0 ? NO_DAMAGED_PASSPORT_ROW_COLOR : BaseColor.WHITE;
                AddTableRow(mainTable, new[]
                {
                    ShapeArabicText(entry.GovernorateName),
                    ShapeArabicText(entry.OfficeName),
                    entry.DamagedPassportCount.ToString()
                }, backgroundColor);
            }

            // Add the main table to the document.
            document.Add(mainTable);

            // Calculate the total damaged passports across all offices.
            var totalDamagedPassportCount = officesWithDamagedPassports.Sum(x => x.DamagedPassportCount);
            var totalFont = FontFactory.GetFont("Amiri", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 12, Font.BOLD);

            // Create the summary cell (green box) for "اجمالي الجوازات"
            var summaryCell = new PdfPCell(new Phrase(ShapeArabicText($"اجمالي الجوازات: {totalDamagedPassportCount}"), totalFont))
            {
                BackgroundColor = TOTAL_BOX_COLOR,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                Padding = 10f,
                BorderColor = BORDER_COLOR,
                RunDirection = PdfWriter.RUN_DIRECTION_RTL
            };

            // Create a one-column table for the summary box that spans the full width of the main table.
            var summaryTable = new PdfPTable(1)
            {
                WidthPercentage = 100,
                SpacingBefore = 10f,
                SpacingAfter = 10f
            };
            summaryTable.AddCell(summaryCell);

            // Add the summary table to the document (it will appear at the bottom).
            document.Add(summaryTable);

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
        private void AddTableRow(PdfPTable table, string[] cells, BaseColor backgroundColor = null)
        {
            if (table == null) throw new ArgumentNullException(nameof(table));
            if (cells == null) throw new ArgumentNullException(nameof(cells));

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
        /// A placeholder for Arabic text shaping.
        /// Replace with proper shaping logic if needed.
        /// </summary>
        private string ShapeArabicText(string text)
        {
            return text;
        }
    }
}
