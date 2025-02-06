// using System.Collections.Generic;
// using System.IO;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Mvc;
// using OMSV1.Domain.Entities.DamagedPassport;
// using OMSV1.Infrastructure.Interfaces;

// namespace OMSV1.Application.Controllers
// {
//     [Route("api/[controller]")]
//     [ApiController]
//     public class ArchiveController : ControllerBase
//     {
//         private readonly IDamagedPassportArchiveService _archiveService;

//         public ArchiveController(IDamagedPassportArchiveService archiveService)
//         {
//             _archiveService = archiveService;
//         }

//         /// <summary>
//         /// For testing: Generates a ZIP archive of damaged passport images from the network share and returns it as a download.
//         /// </summary>
//         /// <returns>ZIP file containing the images.</returns>
//         [HttpGet("test")]
//         public async Task<IActionResult> TestGenerateArchive()
//         {
//             // Define an output directory for the archive (for testing purposes)
//             // Here we're using a TempArchives folder under the current content root.
//             string outputDirectory = Path.Combine(Directory.GetCurrentDirectory(), "TempArchives");

//             // For testing, pass an empty list because our current implementation ignores the damagedPassports parameter.
//             var dummyList = new List<DamagedPassport>();

//             // Generate the archive.
//             var result = await _archiveService.GenerateArchivesAsync(dummyList, outputDirectory);

//             // Try to get the archive path from the returned dictionary.
//             if (result.TryGetValue("all", out string zipFilePath) && System.IO.File.Exists(zipFilePath))
//             {
//                 // Read the file as a byte array.
//                 byte[] fileBytes = await System.IO.File.ReadAllBytesAsync(zipFilePath);

//                 // Return the file with the appropriate content type.
//                 return File(fileBytes, "application/zip", "DamagedPassport_All.zip");
//             }

//             return NotFound("Archive could not be generated.");
//         }
//     }
// }
