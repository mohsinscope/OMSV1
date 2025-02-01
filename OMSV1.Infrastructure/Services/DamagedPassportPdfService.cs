using iTextSharp.text;
using iTextSharp.text.pdf;
using OMSV1.Domain.Entities.DamagedPassport;
using OMSV1.Domain.Interfaces;
using OMSV1.Infrastructure.Interfaces;
using iTextRectangle = iTextSharp.text.Rectangle;

namespace OMSV1.Infrastructure.Services
{
    public class DamagedPassportPdfService : IDamagedPassportService
    {
        private static readonly BaseColor TABLE_HEADER_COLOR = new BaseColor(240, 240, 240);
        private static readonly BaseColor BORDER_COLOR = new BaseColor(120, 120, 120);

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

    // Convert local DateTime.Today to UTC for comparison
    var startOfTodayUtc = DateTime.UtcNow.Date;
    var endOfTodayUtc = startOfTodayUtc.AddDays(1).AddTicks(-1);

    // Filter for passports created today in UTC
    var dailyDamagedPassports = damagedPassports
        .Where(dp => dp.DateCreated >= startOfTodayUtc && dp.DateCreated <= endOfTodayUtc)
        .GroupBy(dp => new
        {
            GovernorateName = dp.Governorate?.Name,
            OfficeName = dp.Office?.Name
        })
        .Select(g => new
        {
            GovernorateName = g.Key.GovernorateName ?? "-",
            OfficeName = g.Key.OfficeName ?? "-",
            DamagedPassportCount = g.Count()
        })
        .ToList();

    if (!dailyDamagedPassports.Any())
        throw new InvalidOperationException("No damaged passports found for today.");

            using var memoryStream = new MemoryStream();
            using var document = new Document(PageSize.A4, 50, 50, 50, 50);
            var writer = PdfWriter.GetInstance(document, memoryStream);

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
            var dateCell = new PdfPCell(new Phrase(ShapeArabicText($"تاريخ التقرير: {DateTime.Now:yyyy-MM-dd}"), dateFont))
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
            var table = new PdfPTable(3)
            {
                WidthPercentage = 100,
                SpacingBefore = 10f,
                SpacingAfter = 20f
            };

            float[] columnWidths = new float[] { 30f, 30f, 40f };
            table.SetWidths(columnWidths);

            AddTableHeader(table, new[]
            {
                ShapeArabicText("عدد الجوازات التالفة"),
                ShapeArabicText("اسم المكتب"),
                ShapeArabicText("اسم المحافظة")
            });

            foreach (var entry in dailyDamagedPassports)
            {
                AddTableRow(table, new[]
                {
                    entry.DamagedPassportCount.ToString(),
                    ShapeArabicText(entry.OfficeName),
                    ShapeArabicText(entry.GovernorateName)
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

        private string ShapeArabicText(string text)
        {
            // Arabic text shaping logic (placeholder; can use libraries like `RTLText`)
            return text; // Replace this with proper shaping logic if needed.
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
    }
}