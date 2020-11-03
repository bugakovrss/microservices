using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartHome.EventlogApi.Contracts;
using SmartHome.Model;
using SmartHome.Model.Entities;
using SmartHome.Model.Errors;

namespace SmartHome.EventlogApi.Controllers
{
    /// <summary>
    /// Управление устройствами
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class DeviceEventsController : ControllerBase
    {
        private readonly IRepository<DeviceEvent> _deviceEventsRepository;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="deviceEventsRepository"></param>
        public DeviceEventsController(IRepository<DeviceEvent> deviceEventsRepository)
        {
            _deviceEventsRepository = deviceEventsRepository;
        }


        /// <summary>
        /// Получить события
        /// </summary>
        /// <param name="deviceId">Идентификатор устройства</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<DeviceEventModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.BadRequest)]
        public async Task<List<DeviceEventModel>> GetEvents([Required] [FromQuery] long deviceId)
        {
            var events = await _deviceEventsRepository.GetAsync(x=>x.DeviceId == deviceId);
            return events.Select(Mapper.Map).OrderBy(x=>x.Time).ToList();
        }

        /// <summary>
        /// Добавить событие
        /// </summary>
        /// <param name="deviceEventModel">Модель события</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(DeviceEventModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.BadRequest)]
        public async Task<DeviceEventModel> AddEvent([Required][FromBody] DeviceEventModel deviceEventModel)
        {
            DeviceEvent device = Mapper.Map(deviceEventModel);

            await _deviceEventsRepository.CreateAsync(device);

            return Mapper.Map(device);
        }

    }
}
