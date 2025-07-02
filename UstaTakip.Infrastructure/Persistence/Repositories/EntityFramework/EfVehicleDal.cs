using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UstaTakip.Application.Repositories;
using UstaTakip.Domain.Entities;
using UstaTakip.Infrastructure.Persistence.Context;

namespace UstaTakip.Infrastructure.Persistence.Repositories.EntityFramework
{
    public class EfVehicleDal : EfEntityRepositoryBase<Vehicle, DataContext>, IVehicleDal
    {
        public EfVehicleDal(DataContext context) : base(context)
        {
        }
        public async Task<List<Vehicle>> GetAllWithDetailsAsync()
        {
            return await _context.Vehicles
                .Include(v => v.Customer)        // Müşteri bilgisi
                .Include(v => v.VehicleImages)   // Araç görselleri
                .ToListAsync();
        }
        public async Task<Vehicle?> GetByIdWithDetailsAsync(Guid id)
        {
            return await _context.Vehicles
                .Include(v => v.Customer)        // Müşteri bilgisi
                .Include(v => v.VehicleImages)   // Araç görselleri
                .FirstOrDefaultAsync(v => v.Id == id);
        }
        public async Task<List<Vehicle>> GetByCustomerIdWithDetailsAsync(Guid customerId)
        {
            return await _context.Vehicles
                .Include(v => v.Customer)
                .Include(v => v.VehicleImages)
                .Where(v => v.CustomerId == customerId)
                .ToListAsync();
        }

    }
}
