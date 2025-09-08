using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using System.Text.Json;
using UstaTakipMvc.Web.Models;
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
            // Son 5 araç (API’de özel endpoint yoksa normal listeyi kullanýyoruz)
            var vehicleResp = await GetApiResponse<List<VehicleListDto>>("vehicles", ct);
            vm.RecentVehicles = (vehicleResp?.Data ?? new()).OrderByDescending(v => v.Id).Take(5).ToList();
            vm.VehicleCount = vehicleResp?.Data?.Count ?? 0;

            // Yakýnda bitecek poliçeler
            var policyResp = await GetApiResponse<List<InsurancePolicyListDto>>("insurancepolicies/expiring?days=30&take=5", ct);
            vm.ExpiringPolicies = policyResp?.Data ?? new();
            vm.PoliciesExpiringSoonCount = vm.ExpiringPolicies.Count;

            // Son 5 tamir kaydý
            var repairResp = await GetApiResponse<List<RepairJobListDto>>("repairjobs/recent?take=5", ct);
            vm.RecentRepairs = repairResp?.Data ?? new();
            vm.OpenRepairCount = vm.RecentRepairs.Count(r => r.Status?.Equals("Open", StringComparison.OrdinalIgnoreCase) == true);

            // Aktif poliçe sayýsý
            var activeCountResp = await GetApiResponse<int>("insurancepolicies/active/count", ct);
            vm.ActivePolicyCount = activeCountResp?.Data ?? 0;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Dashboard verileri alýnamadý (HTTP).");
            TempData["Error"] = "API baðlantý hatasý.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Dashboard verileri alýnamadý.");
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