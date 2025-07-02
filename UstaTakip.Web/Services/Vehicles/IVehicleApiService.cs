using UstaTakip.Web.Models.Vehicles;

namespace UstaTakip.Web.Services.Vehicles
{
    public interface IVehicleApiService
    {
        Task<List<VehicleListDto>> GetAllAsync();
        Task<VehicleListDto?> GetByIdAsync(Guid id);
    }
}
