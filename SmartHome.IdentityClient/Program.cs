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

            var events = RequestDeviceEventsWithPolicy().Result;
            Console.WriteLine($"Принято {events}");

            var events2 = RequestDeviceEventsWithPassword().Result;
            Console.WriteLine($"Принято {events2}");

            Console.ReadLine();
        }

        public static async Task<string> RequestDeviceEventsWithPolicy()
        {

            using (var client = new HttpClient())
            {

                var token = await GetRefreshTokenAsync(client, "http://localhost:5000", "client", "secret", "eventlogapi");
                client.SetBearerToken(token);

                Console.WriteLine($"Bearer {token}");

                var response = await client.GetAsync("http://localhost:8090/api/v1/DeviceEvents?deviceId=1");

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    return response.StatusCode.ToString();
                }

                var content = await response.Content.ReadAsStringAsync();

                return content;
            }
        }

        public static async Task<string> RequestDeviceEventsWithPassword()
        {

            using (var client = new HttpClient())
            {

                var token = await GetRefreshTokenAsync(client, "http://localhost:5000", "ro.client",
                    "secret",
                    "eventlogapi",
                    login: "admin",
                    password: "admin");

                Console.WriteLine($"Bearer {token}");

                client.SetBearerToken(token);

                var response = await client.GetAsync("http://localhost:8090/api/v1/DeviceEvents?deviceId=1");

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    return response.StatusCode.ToString();
                }

                var content = await response.Content.ReadAsStringAsync();

                return content;
            }
        }

        private static async Task<string> GetRefreshTokenAsync( HttpClient client, string authorityUrl, string clientId, string secret, string apiName)
        {
            var disco = await client.GetDiscoveryDocumentAsync(authorityUrl);
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

            return null;
        }

        private static async Task<string> GetRefreshTokenAsync(HttpClient client, string authorityUrl,
            string clientId, string secret, 
            string apiName, string login, string password)
        {
            var disco = await client.GetDiscoveryDocumentAsync(authorityUrl);
            if (disco.IsError) throw new Exception(disco.Error);

            var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = clientId,
                ClientSecret = secret,
                UserName = login,
                Password = password,
                Scope = apiName
            });

            if (!tokenResponse.IsError)
                return tokenResponse.AccessToken;

            return null;
        }
    }
}
