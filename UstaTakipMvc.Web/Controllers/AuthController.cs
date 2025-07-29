using Microsoft.AspNetCore.Mvc;
using UstaTakipMvc.Web.Models.Users;

namespace UstaTakipMvc.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly HttpClient _httpClient;

        public AuthController(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("UstaApi");
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserForRegisterDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("auth/register", dto);
            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Kayıt başarılı, giriş yapabilirsiniz.";
                return RedirectToAction("Login");
            }

            TempData["Error"] = "Kayıt başarısız oldu.";
            return View(dto);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

      

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserForLoginDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("http://localhost:5280/api/Auth/login", dto);
            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Giriş başarılı.";
                return RedirectToAction("Index", "Home");
            }

            TempData["Error"] = "Giriş başarısız. E-posta veya şifre hatalı.";
            return View(dto);
        }
    }
}

