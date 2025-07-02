using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UstaTakip.Domain.Entities;

namespace UstaTakip.Application.Repositories
{
    public interface IVehicleDal:IEntityRepository<Vehicle>
    {
        Task<List<Vehicle>> GetAllWithDetailsAsync();
        Task<Vehicle?> GetByIdWithDetailsAsync(Guid id); // ✅ Yeni eklenen
        Task<List<Vehicle>> GetByCustomerIdWithDetailsAsync(Guid customerId);

    }
}
