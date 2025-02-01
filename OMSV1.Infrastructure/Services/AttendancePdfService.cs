using iTextSharp.text;
using iTextSharp.text.pdf;
using OMSV1.Domain.Entities.Attendances;
using OMSV1.Domain.Enums;
using OMSV1.Domain.Interfaces;
using OMSV1.Infrastructure.Interfaces;
using iTextRectangle = iTextSharp.text.Rectangle;

namespace OMSV1.Infrastructure.Services
{
    public class AttendancePdfService : IAttendanceService
    {
        private static readonly BaseColor TABLE_HEADER_COLOR = new BaseColor(240, 240, 240);
        private static readonly BaseColor BORDER_COLOR = new BaseColor(120, 120, 120);

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
// private void AddTableHeader(PdfPTable table, string[] headers)
// {
//     var headerFont = FontFactory.GetFont("Amiri", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 12);

//     // Add headers in reverse order to ensure RTL display
//     foreach (string header in headers.Reverse())
//     {
//         var cell = new PdfPCell(new Phrase(ShapeArabicText(header), headerFont))
//         {
//             BackgroundColor = TABLE_HEADER_COLOR,
//             HorizontalAlignment = Element.ALIGN_RIGHT,
//             VerticalAlignment = Element.ALIGN_MIDDLE,
//             Padding = 8f,
//             BorderColor = BORDER_COLOR,
//             MinimumHeight = 25f,
//             RunDirection = PdfWriter.RUN_DIRECTION_RTL
//         };
//         table.AddCell(cell);
//     }
// }

// private void AddTableRow(PdfPTable table, string[] cells)
// {
//     var cellFont = FontFactory.GetFont("Amiri", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 10);

//     // Add cells in reverse order to ensure RTL display
//     foreach (string cellContent in cells.Reverse())
//     {
//         var cell = new PdfPCell(new Phrase(ShapeArabicText(cellContent ?? "-"), cellFont))
//         {
//             HorizontalAlignment = Element.ALIGN_RIGHT,
//             VerticalAlignment = Element.ALIGN_MIDDLE,
//             Padding = 6f,
//             BorderColor = BORDER_COLOR,
//             MinimumHeight = 20f,
//             RunDirection = PdfWriter.RUN_DIRECTION_RTL
//         };
//         table.AddCell(cell);
//     }
// }

public async Task<byte[]> GenerateDailyAttendancePdfAsync(List<Attendance> attendances)
{
    if (attendances == null)
        throw new ArgumentNullException(nameof(attendances));

    var todayUtc = DateTime.UtcNow.Date;

    // Aggregate attendance by Governorate and Office, summing up the staff count
    var dailyAttendance = attendances
        .Where(a => a.Date.Date == todayUtc)
        .GroupBy(a => new
        {
            GovernorateName = a.Governorate?.Name,
            OfficeName = a.Office?.Name,
            OfficeCode = a.Office?.Code // Include Office Code in grouping
        })
        .Select(g => new
        {
            GovernorateName = g.Key.GovernorateName ?? "-",
            OfficeName = g.Key.OfficeName ?? "-",
            OfficeCode = g.Key.OfficeCode ?? 0, // Default to 0 if OfficeCode is null
            MorningAttendance = g.Where(a => a.WorkingHours.HasFlag(WorkingHours.Morning))
                .Sum(a => a.ReceivingStaff + a.AccountStaff + a.PrintingStaff + a.QualityStaff + a.DeliveryStaff),
            EveningAttendance = g.Where(a => a.WorkingHours.HasFlag(WorkingHours.Evening))
                .Sum(a => a.ReceivingStaff + a.AccountStaff + a.PrintingStaff + a.QualityStaff + a.DeliveryStaff),
            TotalAttendance = g.Sum(a => a.ReceivingStaff + a.AccountStaff + a.PrintingStaff + a.QualityStaff + a.DeliveryStaff)
        })
        .OrderBy(a => a.OfficeCode) // Sort by Office Code ascendingly
        .ToList();

    if (!dailyAttendance.Any())
        throw new InvalidOperationException("No attendance records found for today.");

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
    var table = new PdfPTable(5) // 5 Columns: Governorate, Office, Morning, Evening, Total
    {
        WidthPercentage = 100,
        SpacingBefore = 10f,
        SpacingAfter = 20f,
        RunDirection = PdfWriter.RUN_DIRECTION_RTL // Ensures full RTL table
    };

    // Column widths adjusted for RTL
    float[] columnWidths = new float[] { 20f, 25f, 15f, 15f, 25f }; // Order: Total, Evening, Morning, Office, Governorate
    table.SetWidths(columnWidths);

    // Add headers in the correct order (from right to left)
    AddTableHeader(table, new[]
    {
        ShapeArabicText("اسم المحافظة"), // Rightmost column
        ShapeArabicText("اسم المكتب"),
        ShapeArabicText("الحضور الصباحي"),
        ShapeArabicText("الحضور المسائي"),
        ShapeArabicText("إجمالي الحضور") // Leftmost column
    });

    // Add rows in the correct order (from right to left)
    foreach (var entry in dailyAttendance)
    {
        AddTableRow(table, new[]
        {
            ShapeArabicText(entry.GovernorateName), // Rightmost column
            ShapeArabicText(entry.OfficeName),
            entry.MorningAttendance.ToString(),
            entry.EveningAttendance.ToString(),
            entry.TotalAttendance.ToString() // Leftmost column
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
    return text; // Replace this with proper Arabic text shaping if needed.
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
