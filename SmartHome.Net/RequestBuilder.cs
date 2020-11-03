using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace SmartHome.Net
{
    public class RequestBuilder
    {
		private readonly HttpClient _httpClient;
		private HttpMethod _method = null;
		private string _url = "";
		private HttpContent _content = null;
		private string _bearerToken = "";
		private string _acceptHeader = "application/json";


        public RequestBuilder(HttpClient httpClient)
		{
            _httpClient = httpClient;
		}

		public RequestBuilder AddMethod(HttpMethod method)
		{
			_method = method;
			return this;
		}

		public RequestBuilder AddUrl(string url)
		{
			_url = url;
			return this;
		}

		public RequestBuilder AddContent(HttpContent content)
		{
			_content = content;
			return this;
		}

		public RequestBuilder AddBearerToken(string bearerToken)
		{
			_bearerToken = bearerToken;
			return this;
		}

		public RequestBuilder AddAcceptHeader(string acceptHeader)
		{
			_acceptHeader = acceptHeader;
			return this;
		}

        public async Task<HttpResponseMessage> SendAsync(CancellationToken token = default)
		{
			var request = new HttpRequestMessage
			{
				Method = _method,
				RequestUri = new Uri(_url)
			};

			if (_content != null)
				request.Content = _content;

			if (!string.IsNullOrEmpty(_bearerToken))
				request.Headers.Authorization =
					new AuthenticationHeaderValue("Bearer", _bearerToken);

			request.Headers.Accept.Clear();
			if (!string.IsNullOrEmpty(_acceptHeader))
				request.Headers.Accept.Add(
					new MediaTypeWithQualityHeaderValue(_acceptHeader));

            return await _httpClient.SendAsync(request, token);
		}
	}
}
