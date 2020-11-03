using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace SmartHome.Net
{
    public static class HttpResponseMessageExtensions
    {
        public static JsonSerializerSettings DefaultSerializeSettings()
        {
            var settings = new JsonSerializerSettings();

            var stringEnumConverter = new StringEnumConverter(new CamelCaseNamingStrategy());

            settings.Converters.Add(stringEnumConverter);

            return settings;
        }
        public static async Task<T> ContentAsAsync<T>(this HttpResponseMessage response)
        {
            var data = await response.Content.ReadAsStringAsync();
            return string.IsNullOrEmpty(data) ?
                default(T) :
                JsonConvert.DeserializeObject<T>(data, DefaultSerializeSettings());
        }

        public static async Task<string> ContentAsStringAsync(this HttpResponseMessage response)
        {
            return await response.Content.ReadAsStringAsync();
        }
	}
}
