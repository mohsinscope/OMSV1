using System.Collections.Generic;
using System.Threading.Tasks;
using OMSV1.Domain.Entities.DamagedPassport;

namespace OMSV1.Infrastructure.Interfaces
{
    public interface IDamagedPassportArchiveService
    {
        /// <summary>
        /// Generates ZIP archives for the provided damaged passports, grouping them by damage type.
        /// Returns a dictionary where the key is a damage type identifier (or name) and the value is the path to the ZIP file.
        /// </summary>
        Task<Dictionary<string, string>> GenerateArchivesAsync(IEnumerable<DamagedPassport> damagedPassports, string outputDirectory);
    }
}
