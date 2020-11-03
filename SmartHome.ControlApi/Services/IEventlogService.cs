using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SmartHome.ControlApi.Services.Model;

namespace SmartHome.ControlApi.Services
{
    public interface IEventlogService
    {
        /// <summary>
        /// Получить события
        /// </summary>
        /// <param name="deviceId">Идентификатор устройства</param>
        /// <param name="token">Токен отмены</param>
        /// <returns></returns>
        Task<List<DeviceEventData>> GetEvents(long deviceId, CancellationToken token = default);

        /// <summary>
        /// Добавить событие
        /// </summary>
        /// <param name="deviceEvent"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task AddEvent(DeviceEventData deviceEvent, CancellationToken token = default);
    }
}
