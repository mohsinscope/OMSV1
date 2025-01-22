using iTextSharp.text;
using iTextSharp.text.pdf;
using OMSV1.Domain.Entities.Expenses;
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
                throw new InvalidOperationException("Error during static initialization of ITextSharpPdfService.", ex);
            }
        }
public async Task<string> GenerateMonthlyExpensesPdfAsync(List<MonthlyExpenses> expenses)
{
    var fileName = $"MonthlyExpenses_{DateTime.Now:yyyyMMddHHmmss}.pdf";
    var outputPath = Path.Combine("C:\\GeneratedPDFs", fileName);

    if (!Directory.Exists("C:\\GeneratedPDFs"))
        Directory.CreateDirectory("C:\\GeneratedPDFs");

    using var fs = new FileStream(outputPath, FileMode.Create);
    using var document = new Document(PageSize.A4, 50, 50, 50, 50);
    var writer = PdfWriter.GetInstance(document, fs);

    document.Open();

    // Add Title
    var titleFont = FontFactory.GetFont("Amiri", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 24);
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

    // Add Report Generation Date
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

    // Create and style the table for the report data
    var table = new PdfPTable(5) // Updated to 5 columns
    {
        WidthPercentage = 100,
        SpacingBefore = 10f,
        SpacingAfter = 20f
    };

    // Set column widths
    float[] columnWidths = new float[] { 20f, 20f, 20f, 20f, 20f }; // Adjust widths to fit 5 columns
    table.SetWidths(columnWidths);

    // Add table headers
    AddTableHeader(table, new[]
    {
        ShapeArabicText("المبلغ الإجمالي"),
        ShapeArabicText("اسم المشرف"),
        ShapeArabicText("اسم المكتب"),
        ShapeArabicText("اسم المحافظة"),
        ShapeArabicText("تاريخ الإنشاء") // New column for DateCreated
    });

    // Add table rows
    foreach (var expense in expenses)
    {
        AddTableRow(table, new[]
        {
            expense.TotalAmount.ToString("C"),
            ShapeArabicText(expense.Profile.FullName),
            ShapeArabicText(expense.Office.Name),
            ShapeArabicText(expense.Governorate.Name),
            ShapeArabicText(expense.DateCreated.ToString("yyyy-MM-dd")) // Format DateCreated
        });
    }

    document.Add(table);

    // Add Footer
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
    return outputPath;
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
            HorizontalAlignment = Element.ALIGN_RIGHT, // Ensure RTL alignment
            VerticalAlignment = Element.ALIGN_MIDDLE,
            Padding = 8f,
            BorderColor = BORDER_COLOR,
            MinimumHeight = 25f,
            RunDirection = PdfWriter.RUN_DIRECTION_RTL // Set to RTL
        };
        table.AddCell(cell);
    }
}


private void AddTableRow(PdfPTable table, string[] cells)
{
    var cellFont = FontFactory.GetFont("Amiri", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 10);

    foreach (string cellContent in cells)
    {
        var cell = new PdfPCell(new Phrase(ShapeArabicText(cellContent), cellFont))
        {
            HorizontalAlignment = Element.ALIGN_RIGHT, // Ensure RTL alignment
            VerticalAlignment = Element.ALIGN_MIDDLE,
            Padding = 6f,
            BorderColor = BORDER_COLOR,
            MinimumHeight = 20f,
            RunDirection = PdfWriter.RUN_DIRECTION_RTL // Set to RTL
        };
        table.AddCell(cell);
    }
}

    }
}
