using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.Extensions.Options;
using SmartHome.AuthApi.Configuration;

namespace SmartHome.AuthApi.Services
{
    public class AuthService: IAuthService
    {
        private readonly HttpClient _client;
        private readonly IdentityServerSettings _serverSettings;
        public AuthService(IOptions<IdentityServerSettings> options, HttpClient client)
        {
            _client = client;
            _serverSettings = options.Value;
        }

        public async Task<string> Authenticate(string login, string password, IEnumerable<string> scopes, CancellationToken token = default)
        {
            var discoverRequest = new DiscoveryDocumentRequest
            {
                Address = _serverSettings.Host, Policy =
                {
                    RequireHttps = false,
                    ValidateIssuerName = false,
                    ValidateEndpoints = false
                }
            };


            var disco = await _client.GetDiscoveryDocumentAsync(discoverRequest
                , token);

            if (disco.IsError) 
                throw new Exception(disco.Error);

            var tokenResponse = await _client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = _serverSettings.Client,
                ClientSecret = _serverSettings.Secret,
                UserName = login,
                Password = password,
                Scope = string.Join(" ", scopes ?? Enumerable.Empty<string>())
            }, cancellationToken: token);


            if (tokenResponse.IsError)
            {
                throw new Exception(tokenResponse.Error);
            }

            return tokenResponse.AccessToken;
        }
    }
}
