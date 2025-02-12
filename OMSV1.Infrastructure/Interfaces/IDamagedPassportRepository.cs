using OMSV1.Domain.Entities.DamagedPassport;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OMSV1.Domain.Interfaces
{
    public interface IDamagedPassportRepository
    {
        //Task<List<DamagedPassport>> GetAllDamagedPassportsAsync();
        Task<List<DamagedPassport>> GetDamagedPassportsByDateAsync(DateTime date);
        Task<List<DamagedPassport>> GetDamagedPassportsByDateCreatedAsync(DateTime date);

    }
}