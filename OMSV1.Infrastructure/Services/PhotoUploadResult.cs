using System;

namespace OMSV1.Application.Dtos.Attachments;

public class PhotoUploadResult
{
    public string FilePath { get; set; } // The relative or absolute URL of the uploaded file
    public string FileName { get; set; } // The unique file name used for storage
}
