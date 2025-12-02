using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using System.Text.Json;
using UstaTakipMvc.Web.Models;
using UstaTakipMvc.Web.Models.Customers;
using UstaTakipMvc.Web.Models.Dashboards;
using UstaTakipMvc.Web.Models.InsurancePolicies;
using UstaTakipMvc.Web.Models.RepairJobs;
using UstaTakipMvc.Web.Models.Shared;
using UstaTakipMvc.Web.Models.Vehicles;

namespace UstaTakipMvc.Web.Controllers;


[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly HttpClient _api;
    private static readonly JsonSerializerOptions _json = new(JsonSerializerDefaults.Web);

    public HomeController(ILogger<HomeController> logger, IHttpClientFactory factory)
    {
        _logger = logger;
        _api = factory.CreateClient("UstaApi");
        _api.DefaultRequestHeaders.Accept.ParseAdd("application/json");
    }
    public async Task<IActionResult> Index(CancellationToken ct = default)
    {
        var vm = new DashboardViewModel();

        try
        {
            // =============================
            // 1) Araçlar
            // =============================
            var vehicleResp = await GetApiResponse<List<VehicleListDto>>("vehicles", ct);
            vm.VehicleCount = vehicleResp?.Data?.Count ?? 0;
            vm.RecentVehicles = vehicleResp?.Data?
                .OrderByDescending(v => v.Id)
                .Take(5)
                .ToList() ?? new();


            // =============================
            // 2) Müþteriler
            // =============================
            var customerResp = await GetApiResponse<List<CustomerListDto>>("customers", ct);
            vm.CustomerCount = customerResp?.Data?.Count ?? 0;


            // =============================
            // 3) Tamir Ýþleri (Open + InProgress + Completed)
            // =============================
            var repairResp = await GetApiResponse<List<RepairJobListDto>>("repairjobs", ct);

            var repairs = repairResp?.Data ?? new List<RepairJobListDto>();

            vm.OpenRepairCount = repairs.Count(r => r.Status == "Open");
            vm.InProgressRepairCount = repairs.Count(r => r.Status == "InProgress");

            vm.RecentRepairs = repairs
                .OrderByDescending(r => r.Date)
                .Take(5)
                .ToList();


            // =============================
            // 4) Yaklaþan Poliçeler (30 gün)
            // =============================
            var expiringResp = await GetApiResponse<List<InsurancePolicyListDto>>("insurancepolicies/expiring?days=30&take=5", ct);

            vm.ExpiringPolicies = expiringResp?.Data ?? new();
            vm.ExpiringPolicyCount = vm.ExpiringPolicies.Count;


            // =============================
            // 5) Aylýk Tamir Ýstatistikleri (Grafik)
            // =============================
            var monthlyResp = await GetApiResponse<List<MonthlyRepairJobDto>>(
      "repairjobs/GetJobMonthly", ct);

            vm.MonthlyStats = monthlyResp?.Data ?? new();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Dashboard verileri çekilirken hata oluþtu.");
            TempData["Error"] = "Dashboard verileri alýnýrken bir hata oluþtu.";
        }

        return View(vm);
    }


    private async Task<ApiDataResult<T>?> GetApiResponse<T>(string endpoint, CancellationToken ct)
    {
        var resp = await _api.GetAsync(endpoint, ct);
        if (!resp.IsSuccessStatusCode)
        {
            _logger.LogWarning("API {Status} döndü: {Endpoint}", (int)resp.StatusCode, endpoint);
            return null;
        }

        try
        {
            return await resp.Content.ReadFromJsonAsync<ApiDataResult<T>>(_json, ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "API yanýtý parse edilemedi: {Endpoint}", endpoint);
            return null;
        }
    }

    [AllowAnonymous]
    public IActionResult Privacy() => View();

    [AllowAnonymous]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() =>
        View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}