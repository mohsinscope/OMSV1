using iTextSharp.text;
using iTextSharp.text.pdf;
using OMSV1.Domain.Entities.DamagedPassport;
using OMSV1.Domain.Entities.Expenses;
using OMSV1.Domain.Enums;
using OMSV1.Infrastructure.Interfaces;
using iTextRectangle = iTextSharp.text.Rectangle;

namespace OMSV1.Infrastructure.Services
{
    public class ITextSharpPdfService : IPdfService
    {
        private static readonly BaseColor TABLE_HEADER_COLOR = new BaseColor(240, 240, 240);
        private static readonly BaseColor BORDER_COLOR = new BaseColor(120, 120, 120);

        static ITextSharpPdfService()
        {
            try
            {
                // Register additional encoding providers
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

                // Register Times New Roman font
                var fontPath = @"C:\Windows\Fonts\times.ttf";
                if (!System.IO.File.Exists(fontPath))
                {
                    throw new FileNotFoundException($"Font file not found at {fontPath}");
                }
                FontFactory.Register(fontPath, "Times New Roman");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error during static initialization of ITextSharpPdfService.", ex);
            }
        }

        public async Task<byte[]> GenerateMonthlyExpensesPdfAsync(List<MonthlyExpenses> expenses)
        {
            if (expenses == null)
                throw new ArgumentNullException(nameof(expenses));

            // Filter for completed expenses
            var completedExpenses = expenses.Where(e => e.Status == Status.Completed).ToList();

            if (!completedExpenses.Any())
                throw new InvalidOperationException("No completed expenses found for the specified period.");

            using var memoryStream = new MemoryStream();
            using var document = new Document(PageSize.A4, 50, 50, 50, 50);
            var writer = PdfWriter.GetInstance(document, memoryStream);

            document.Open();

            // Title Table
            var titleFont = FontFactory.GetFont("Times New Roman", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 24);
            var titleCell = new PdfPCell(new Phrase(ShapeArabicText("تقرير المصروفات الشهرية"), titleFont))
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
            var dateFont = FontFactory.GetFont("Times New Roman", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 12);
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
            var table = new PdfPTable(5)
            {
                WidthPercentage = 100,
                SpacingBefore = 10f,
                SpacingAfter = 20f
            };

            float[] columnWidths = new float[] { 20f, 20f, 20f, 20f, 20f };
            table.SetWidths(columnWidths);

            AddTableHeader(table, new[]
            {
                ShapeArabicText("المبلغ الإجمالي"),
                ShapeArabicText("اسم المشرف"),
                ShapeArabicText("اسم المكتب"),
                ShapeArabicText("اسم المحافظة"),
                ShapeArabicText("تاريخ الإنشاء")
            });

            foreach (var expense in completedExpenses)
            {
                AddTableRow(table, new[]
                {
                    expense?.TotalAmount.ToString("C") ?? "-",
                    ShapeArabicText(expense?.Profile?.FullName ?? "-"),
                    ShapeArabicText(expense?.Office?.Name ?? "-"),
                    ShapeArabicText(expense?.Governorate?.Name ?? "-"),
                    ShapeArabicText(expense?.DateCreated.ToString("yyyy-MM-dd") ?? "-")
                });
            }

            document.Add(table);

            // Footer Table
            var footerFont = FontFactory.GetFont("Times New Roman", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 8);
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

            var cellFont = FontFactory.GetFont("Times New Roman", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 10);

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
            var headerFont = FontFactory.GetFont("Times New Roman", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 12);

            foreach (string header in headers)
            {
                var cell = new PdfPCell(new Phrase(ShapeArabicText(header), headerFont))
                {
                    BackgroundColor = TABLE_HEADER_COLOR,
                    HorizontalAlignment = Element.ALIGN_RIGHT, // Ensure RTL alignment
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
