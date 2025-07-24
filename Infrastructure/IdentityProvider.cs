using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using OperationWorker.Core.Abstractions.Auth;
using OperationWorker.Core.DTOs;
using System.Text.Json;

namespace OperationWorker.Infrastructure
{
    public class IdentityProvider : IIdentityProvider
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IdentityProvider(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        private readonly string IdentityServerURL = "https://localhost:10001/";

        public async Task<IdentityResponseDTO> RequestToken(string login, string password)
        {
            var response = new IdentityResponseDTO();
            var identityHttpClient = _httpClientFactory.CreateClient();

            DiscoveryDocumentResponse discoveryDocumentResponse = await identityHttpClient.GetDiscoveryDocumentAsync(IdentityServerURL);
            if (!discoveryDocumentResponse.IsError)
            {
                var tokenResponse = await identityHttpClient.RequestPasswordTokenAsync(new PasswordTokenRequest
                {
                    Address = discoveryDocumentResponse.TokenEndpoint,
                    ClientId = "GfAppClient",
                    ClientSecret = _configuration["Clients:GfAppClient:client_secret"],
                    UserName = login,
                    Password = password,
                    Scope = "api.full_access offline_access"
                });
                if (tokenResponse.IsError)
                {
                    response.IsCompleted = false;
                    response.ErrorMessage = $"Authorization error: {tokenResponse.Error} - {tokenResponse.ErrorDescription}";
                }
                else
                {
                    response.IsCompleted = true;
                    response.AccessToken = tokenResponse.AccessToken;
                    response.RefreshToken = tokenResponse.RefreshToken;
                }
            }
            else
            {
                response.IsCompleted = false;
                response.ErrorMessage = $"Authorization error: {discoveryDocumentResponse.Error} - {discoveryDocumentResponse.ErrorType}";
            }
            return response;
        }

        public async Task<IdentityResponseDTO> RequestTokenWithHeaders(string login, string password, CancellationToken ct)
        {
            var response = new IdentityResponseDTO();
            var identityHttpClient = _httpClientFactory.CreateClient();

            var discovery = await identityHttpClient.GetDiscoveryDocumentAsync(IdentityServerURL, ct);
            if (discovery.IsError)
            {
                response.IsCompleted = false;
                response.ErrorMessage = $"Discovery Document error: {discovery.Error}";
                return response;
            }

            var request = new HttpRequestMessage(HttpMethod.Post, discovery.TokenEndpoint)
            {
                Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "client_id", "GfAppClient" },
                    { "client_secret", _configuration["Clients:GfAppClient:client_secret"]! },
                    { "grant_type", "password" },
                    { "username", login },
                    { "password", password },
                    { "scope", "api.full_access offline_access" }
                })
            };

            // Прокидываем заголовки
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                if (httpContext.Request.Headers.TryGetValue("User-Agent", out var userAgent))
                    request.Headers.TryAddWithoutValidation("User-Agent", userAgent.ToString());

                var ip = httpContext.Connection?.RemoteIpAddress?.ToString();
                if (!string.IsNullOrWhiteSpace(ip))
                    request.Headers.TryAddWithoutValidation("X-Forwarded-For", ip);
            }

            try
            {
                var tokenHttpResponse = await identityHttpClient.SendAsync(request, ct);
                var content = await tokenHttpResponse.Content.ReadAsStringAsync();

                if (!tokenHttpResponse.IsSuccessStatusCode)
                {
                    response.IsCompleted = false;
                    response.ErrorMessage = $"Token error: {tokenHttpResponse.StatusCode} - {content}";
                    return response;
                }

                var tokenResponse = JsonSerializer.Deserialize<TokenResponseDTO>(content);
                if (tokenResponse != null)
                {
                    response.IsCompleted = true;
                    response.AccessToken = tokenResponse.AccessToken;
                    response.RefreshToken = tokenResponse.RefreshToken;
                }
                else
                {
                    response.IsCompleted = false;
                    response.ErrorMessage = "Failed to deserialize token response.";
                }
            }
            catch (Exception ex)
            {
                response.IsCompleted = false;
                response.ErrorMessage = $"Unexpected error: {ex.Message}";
            }

            return response;
        }

        public async Task<IdentityResponseDTO> RefreshAccessToken(string refreshToken)
        {
            var response = new IdentityResponseDTO();
            var identityHttpClient = _httpClientFactory.CreateClient();

            DiscoveryDocumentResponse discoveryDocumentResponse = await identityHttpClient.GetDiscoveryDocumentAsync(IdentityServerURL);
            if (!discoveryDocumentResponse.IsError)
            {
                var tokenResponse = await identityHttpClient.RequestRefreshTokenAsync(new RefreshTokenRequest
                {
                    Address = discoveryDocumentResponse.TokenEndpoint,
                    RefreshToken = refreshToken,
                    ClientId = "GfAppClient",
                    ClientSecret = _configuration["Clients:GfAppClient:client_secret"],
                });
                if (tokenResponse.IsError)
                {
                    response.IsCompleted = false;
                    response.ErrorMessage = $"Authorization error: {tokenResponse.Error} - {tokenResponse.ErrorDescription}";
                }
                else
                {
                    response.IsCompleted = true;
                    response.AccessToken = tokenResponse.AccessToken;
                }
            }
            else
            {
                response.IsCompleted = false;
                response.ErrorMessage = $"Authorization error: {discoveryDocumentResponse.Error} - {discoveryDocumentResponse.ErrorType}";
            }
            return response;
        }
    }
}
