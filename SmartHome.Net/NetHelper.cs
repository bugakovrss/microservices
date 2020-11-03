using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SmartHome.Net
{
	public static class NetHelper
	{
        public static async Task<HttpResponseMessage> GetAsync(HttpClient client,
            string url,
            CancellationToken token = default)
        {
            var builder = new RequestBuilder(client)
                .AddMethod(HttpMethod.Get)
                .AddUrl(url);

            return await builder.SendAsync(token);
        }


		public static async Task<HttpResponseMessage> PostAsync(HttpClient client,
			string url, object value = null,
			CancellationToken token = default)
		{
			var builder = new RequestBuilder(client)
				.AddMethod(HttpMethod.Post)
                .AddUrl(url);

			if (value != null)
			{
				builder.AddContent(new JsonContent(value ?? new { }));
			}

			return await builder.SendAsync(token);
		}


		public static async Task<HttpResponseMessage> PutAsync(
			HttpClient client,
			string url, object value,
			CancellationToken token = default)
		{
			var builder = new RequestBuilder(client)
				.AddMethod(HttpMethod.Put)
				.AddUrl(url)
				.AddContent(new JsonContent(value ?? new { }));

			return await builder.SendAsync(token);
		}


		public static async Task<HttpResponseMessage> DeleteAsync(HttpClient client,
            string url,
			CancellationToken token = default)
		{
			var builder = new RequestBuilder(client)
				.AddMethod(HttpMethod.Delete)
				.AddUrl(url);

			return await builder.SendAsync(token);
		}

	}
}
