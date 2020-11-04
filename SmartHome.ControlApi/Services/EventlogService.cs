using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SmartHome.ControlApi.Configuration;
using SmartHome.ControlApi.ErrorHandling;
using SmartHome.ControlApi.Services.Model;
using SmartHome.Model.Errors;
using SmartHome.Net;

namespace SmartHome.ControlApi.Services
{
    public class EventlogService: IEventlogService
    {
        private readonly HttpClient _client;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly EventlogApiEndpoints _endpoints;

        public EventlogService(IOptions<EventlogApiEndpoints> endpoints, HttpClient client, IHttpContextAccessor contextAccessor)
        {
            _client = client;
            _contextAccessor = contextAccessor;
            _endpoints = endpoints.Value;
        }


        public async Task<List<DeviceEventData>> GetEvents(long deviceId, CancellationToken token)
        {


            var url = $"{_endpoints.Host}{_endpoints.GetEvents}?deviceId={deviceId}";

            var bearer = await GetBearerToken();

            var messageResponse = await NetHelper.GetAsync(_client, url, bearer, token);

            if (!messageResponse.IsSuccessStatusCode)
            {
                if (messageResponse.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new ControlApiException(await messageResponse.ContentAsStringAsync(), ErrorCode.Unauthorized);
                }

                throw await messageResponse.ReadErrorAsync();
            }

            return await messageResponse.ContentAsAsync<List<DeviceEventData>>();
        }

        public async Task AddEvent(DeviceEventData deviceEvent, CancellationToken token)
        {
            var url = $"{_endpoints.Host}{_endpoints.AddEvent}";

            var bearer = await GetBearerToken();

            var messageResponse = await NetHelper.PostAsync(_client, url, bearer, deviceEvent, token);

            if (!messageResponse.IsSuccessStatusCode)
            {
                if (messageResponse.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new ControlApiException(await messageResponse.ContentAsStringAsync(), ErrorCode.Unauthorized);
                }

                throw await messageResponse.ReadErrorAsync();
            }

            var addedEvent = await messageResponse.ContentAsAsync<DeviceEventData>();
            deviceEvent.Id = addedEvent.Id;
        }

        private Task<string> GetBearerToken()
        {
           return _contextAccessor.HttpContext.GetTokenAsync(JwtBearerDefaults.AuthenticationScheme,
                "access_token");
        }
    }
}
