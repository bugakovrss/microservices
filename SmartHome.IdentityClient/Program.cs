using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;

namespace SmartHome.IdentityClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var data = RequestDeviceEventsWithPolicy().Result;
            Console.ReadLine();
        }

        public static async Task<string> RequestDeviceEventsWithPolicy()
        {

            using (var client = new HttpClient())
            {

                var token = await GetRefreshTokenAsync(client, "http://localhost:5000", "client", "secret", "eventlogapi");
                client.SetBearerToken(token);

                var response = await client.GetAsync("http://localhost:8090/api/v1/DeviceEvents?deviceId=1");

                if (!response.IsSuccessStatusCode)
                {
                    return response.StatusCode.ToString();
                }

                var content = await response.Content.ReadAsStringAsync();

                return content;
            }
        }

        private static async Task<string> GetRefreshTokenAsync( HttpClient client, string authority, string clientId, string secret, string apiName)
        {
            var disco = await client.GetDiscoveryDocumentAsync(authority);
            if (disco.IsError) throw new Exception(disco.Error);

            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = clientId,
                ClientSecret = secret,
                Scope = apiName
            });

            if (!tokenResponse.IsError) 
                return tokenResponse.AccessToken;

            var error = tokenResponse.Error;
            return null;
        }
    }
}
