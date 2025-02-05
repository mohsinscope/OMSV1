using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using OMSV1.Infrastructure.Interfaces;

namespace OMSV1.Infrastructure.Services
{
    public class DamagedPassportArchiveService : IDamagedPassportArchiveService
    {
        /// <summary>
        /// Creates a ZIP archive in memory containing all .jpg files from the source folder.
        /// Returns a dictionary with a key ("all") and a temporary file path where the archive is stored.
        /// </summary>
        public async Task<Dictionary<string, string>> GenerateArchivesAsync(
            IEnumerable<Domain.Entities.DamagedPassport.DamagedPassport> damagedPassports, 
            string outputDirectory)
        {
            // Define the source folder.
            // Change this to the exact folder where your damaged passport images are stored.
            string sourceFolder = Path.Combine(@"\\172.16.108.26\samba", "damagedpassport");

            // Log the source folder path.
            Console.WriteLine($"Using source folder: {sourceFolder}");
            
            // Check if the source folder exists.
            if (!Directory.Exists(sourceFolder))
            {
                Console.WriteLine($"Source folder does not exist: {sourceFolder}");
                return new Dictionary<string, string>();
            }
            
            // List all files in the folder (you can log everything to verify the files exist).
            string[] allFiles = Directory.GetFiles(sourceFolder);
            Console.WriteLine($"Found {allFiles.Length} files in the folder (without filter):");
            foreach (var file in allFiles)
            {
                Console.WriteLine($"  {file}");
            }
            
            // Use a filter for .jpg files.
            string[] files = Directory.GetFiles(sourceFolder, "*.jpg");
            Console.WriteLine($"Found {files.Length} .jpg files in the folder:");
            foreach (var file in files)
            {
                Console.WriteLine($"  {file}");
            }
            
            // Ensure the output directory exists.
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Define the output ZIP file name and path.
            string zipFileName = "DamagedPassport_All.zip";
            string zipFilePath = Path.Combine(outputDirectory, zipFileName);

            // Create the ZIP archive.
            using (var zipStream = new FileStream(zipFilePath, FileMode.Create))
            using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Create))
            {
                foreach (var file in files)
                {
                    // Use the file's name as the entry name.
                    string entryName = Path.GetFileName(file);
                    Console.WriteLine($"Adding file to archive: {entryName}");

                    var entry = archive.CreateEntry(entryName, CompressionLevel.Optimal);
                    using (var entryStream = entry.Open())
                    using (var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read))
                    {
                        await fileStream.CopyToAsync(entryStream);
                    }
                }
            }

            Console.WriteLine($"ZIP archive created at: {zipFilePath}");

            return new Dictionary<string, string> { { "all", zipFilePath } };
        }
    }
}
