using OMSV1.Domain.Entities.DamagedPassport;
using OMSV1.Domain.Entities.Expenses;

namespace OMSV1.Infrastructure.Interfaces
{
 public interface IDamagedPassportService
{
    Task<byte[]> GenerateDailyDamagedPassportsPdfAsync(List<DamagedPassport> damagedPassports);
}

}
