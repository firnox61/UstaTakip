using UstaTakip.Core.Utilities.Results;
using UstaTakip.Web.Models.Shared;
using UstaTakip.Web.Models.Vehicles;

namespace UstaTakip.Web.Services.Vehicles
{
    public class VehicleApiService : IVehicleApiService
    {
        private readonly HttpClient _http;

        public VehicleApiService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<VehicleListDto>> GetAllAsync()
        {
            var response = await _http.GetFromJsonAsync<ApiDataResult<List<VehicleListDto>>>("api/vehicles");

            return response?.Data ?? new();
        }

        public async Task<VehicleListDto?> GetByIdAsync(Guid id)
        {
            var response = await _http.GetFromJsonAsync<ApiDataResult<VehicleListDto>>($"api/vehicles/{id}");

            return response?.Data;
        }
    }

}
