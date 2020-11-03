using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SmartHome.ControlApi.Configuration;
using SmartHome.ControlApi.ErrorHandling;
using SmartHome.ControlApi.Services.Model;
using SmartHome.Net;

namespace SmartHome.ControlApi.Services
{
    public class EventlogService: IEventlogService
    {
        private readonly HttpClient _client;
        private readonly EventlogApiEndpoints _endpoints;

        public EventlogService(IOptions<EventlogApiEndpoints> endpoints, HttpClient client)
        {
            _client = client;
            _endpoints = endpoints.Value;
        }

        public async Task<List<DeviceEventData>> GetEvents(long deviceId, CancellationToken token)
        {
            var url = $"{_endpoints.Host}{_endpoints.GetEvents}?deviceId={deviceId}";

            var messageResponse = await NetHelper.GetAsync(_client, url, token);

            if (!messageResponse.IsSuccessStatusCode)
            {
                throw await messageResponse.ReadErrorAsync();
            }

            return await messageResponse.ContentAsAsync<List<DeviceEventData>>();
        }

        public async Task AddEvent(DeviceEventData deviceEvent, CancellationToken token)
        {
            var url = $"{_endpoints.Host}{_endpoints.AddEvent}";

            var messageResponse = await NetHelper.PostAsync(_client, url, deviceEvent, token);

            if (!messageResponse.IsSuccessStatusCode)
            {
                throw await messageResponse.ReadErrorAsync();
            }

            var addedEvent = await messageResponse.ContentAsAsync<DeviceEventData>();
            deviceEvent.Id = addedEvent.Id;
        }
    }
}
