using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Requests;
using Google.Apis.Auth.OAuth2.Responses;
using OAuthTest.Models.GoogleOAuth;

namespace OAuthTest.Services
{
    public class OIDCService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public OIDCService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<string> GetIdTokenAsync(string authorization_code)
        {
            using HttpClient client = _httpClientFactory.CreateClient();
            AuthorizationCodeTokenRequest request = new()
            {
                Code = authorization_code,
                RedirectUri = "https://localhost:7228/api/auth/oidc/signin",
                ClientId = _configuration["Google:ClientId"],
                ClientSecret = _configuration["Google:ClientSecret"],
                Scope = "https://www.googleapis.com/auth/userinfo.email"
            };

            TokenResponse responce = await request.ExecuteAsync(client, GoogleAuthConsts.OidcTokenUrl, new(), Google.Apis.Util.SystemClock.Default);

            return responce.IdToken;
        }

        public async Task<UserInfo?> GetTokenInfo(string token)
        {
            string url = $"https://oauth2.googleapis.com/tokeninfo?id_token={token}";

            HttpRequestMessage httpRequestMessage = new(HttpMethod.Get, url);

            var httpClient = _httpClientFactory.CreateClient();
            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
            var userInfo = await httpResponseMessage.Content.ReadFromJsonAsync<UserInfo>();
            if (userInfo is null)
            {
                return null;
            }
            return userInfo;
        }
    }
}