using System.Net.Http.Headers;
using UstaTakip.Web.Models.Users;

namespace UstaTakip.Web.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _http;
        private AccessToken? _accessToken;

        public AuthService(HttpClient http)
        {
            _http = http;
        }

        public bool IsAuthenticated => _accessToken != null;

        public async Task<bool> LoginAsync(UserForLoginDto loginModel)
        {
            if (string.IsNullOrWhiteSpace(loginModel.Email) || string.IsNullOrWhiteSpace(loginModel.Password))
                return false;
            var response = await _http.PostAsJsonAsync("http://localhost:5280/api/Auth/login", loginModel);
            return response.IsSuccessStatusCode;
            


            if (!response.IsSuccessStatusCode)
                return false;

            _accessToken = await response.Content.ReadFromJsonAsync<AccessToken>();

            // API isteklerinde token kullanılsın
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken.Token);

            return true;
        }

        public async Task LogoutAsync()
        {
            _accessToken = null;
            _http.DefaultRequestHeaders.Authorization = null;
        }

        public async Task<string?> GetTokenAsync()
        {
            return _accessToken?.Token;
        }
    }
}
