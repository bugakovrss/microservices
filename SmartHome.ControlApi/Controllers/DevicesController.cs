using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartHome.ControlApi.Contracts;
using SmartHome.ControlApi.ErrorHandling;
using SmartHome.ControlApi.Services;
using SmartHome.ControlApi.Services.Model;
using SmartHome.Model;
using SmartHome.Model.Entities;
using SmartHome.Model.Errors;
using SmartHome.Model.Helpers;

namespace SmartHome.ControlApi.Controllers
{
    /// <summary>
    /// Управление устройствами
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class DevicesController : ControllerBase
    {
        private readonly IRepository<Device> _deviceRepository;
        private readonly IEventlogService _eventLogService;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="deviceRepository"></param>
        /// <param name="eventLogService"></param>
        public DevicesController(IRepository<Device> deviceRepository, IEventlogService eventLogService)
        {
            _deviceRepository = deviceRepository;
            _eventLogService = eventLogService;
        }

        /// <summary>
        /// Получить список устройства
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<DeviceModel>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.BadRequest)]
        public async Task<List<DeviceModel>> GetDevices()
        {
            var devices = await _deviceRepository.GetAsync(x => true);
            return devices.Select(Mapper.Map).ToList();
        }

        /// <summary>
        /// Получить устройство
        /// </summary>
        /// <param name="deviceId">Идентификатор устройства</param>
        /// <returns></returns>
        [HttpGet("{deviceId}")]
        [ProducesResponseType(typeof(DeviceModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.BadRequest)]
        public async Task<DeviceModel> GetDevice([Required] [FromRoute] long deviceId)
        {
            var device = await _deviceRepository.GetAsync(deviceId);
            return Mapper.Map(device);
        }

        /// <summary>
        /// Получить лог событий
        /// </summary>
        /// <param name="deviceId">Идентификатор устройства</param>
        /// <returns></returns>
        [HttpGet("{deviceId}/Events")]
        [ProducesResponseType(typeof(DeviceEventsResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.BadRequest)]
        public async Task<DeviceEventsResponse> GetDeviceEvents([Required][FromRoute] long deviceId)
        {
            var device = await _deviceRepository.GetAsync(deviceId);

            if(device == null)
                throw new ControlApiException("Устройство не найдено", ErrorCode.NotFound);


            var events = await _eventLogService.GetEvents(deviceId, HttpContext.RequestAborted);

            return new DeviceEventsResponse
            {
                Id = deviceId,
                State = device.State,
                Name = device.Name,
                Events = events?.Select(Mapper.Map).ToList()
            };
        }

        /// <summary>
        /// Удалить устройство
        /// </summary>
        /// <param name="deviceId">Идентификатор устройства</param>
        /// <returns></returns>
        [HttpDelete("{deviceId}")]
        [ProducesResponseType(typeof(StatusResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.BadRequest)]
        public async Task<StatusResponse> DeleteDevice([Required][FromRoute] long deviceId)
        {
             await _deviceRepository.DeleteAsync(deviceId);
             return new StatusResponse {Status = "Ok"};
        }

        /// <summary>
        /// Создать устройство
        /// </summary>
        /// <param name="deviceModel">Модель устройства</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(DeviceModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.BadRequest)]
        public async Task<DeviceModel> CreateDevice([Required] [FromBody] DeviceModel deviceModel)
        {
            Device device = Mapper.Map(deviceModel);

            await _deviceRepository.CreateAsync(device);

            return Mapper.Map(device);
        }


        /// <summary>
        /// Включить устройство
        /// </summary>
        /// <param name="deviceId">Идентификатор устройства</param>
        /// <returns></returns>
        [HttpPost("TurnOn/{deviceId}")]
        [ProducesResponseType(typeof(StatusResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.BadRequest)]
        public async Task<StatusResponse> TurnOnDevice([Required][FromRoute] long deviceId)
        {

            var device = await _deviceRepository.GetAsync(deviceId);
            if(device == null)
                throw new ControlApiException("Устройство не найдено",ErrorCode.NotFound);

            if(device.State == DeviceState.On)
                throw new ControlApiException("Устройство уже включено", ErrorCode.WrongOperation);

            device.State = DeviceState.On;

            await _deviceRepository.UpdateAsync(device);

            await _eventLogService.AddEvent(new DeviceEventData
            {
                DeviceId = deviceId,
                Message = MessageHelper.GetMesssage(EventType.TurnedOn),
                Type = EventType.TurnedOn,
                Time = DateTime.Now
            },  HttpContext.RequestAborted);

            return new StatusResponse { Status = "Ok" };
        }


        /// <summary>
        /// Выключить устройство
        /// </summary>
        /// <param name="deviceId">Идентификатор устройства</param>
        /// <returns></returns>
        [HttpPost("TurnOff/{deviceId}")]
        [ProducesResponseType(typeof(StatusResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.BadRequest)]
        public async Task<StatusResponse> TurnOffDevice([Required][FromRoute] long deviceId)
        {

            var device = await _deviceRepository.GetAsync(deviceId);
            if (device == null)
                throw new ControlApiException("Устройство не найдено", ErrorCode.NotFound);

            if (device.State == DeviceState.Off)
                throw new ControlApiException("Устройство уже выключено", ErrorCode.WrongOperation);

            device.State = DeviceState.Off;

            await _deviceRepository.UpdateAsync(device);

            await _eventLogService.AddEvent(new DeviceEventData
            {
                DeviceId = deviceId,
                Message = MessageHelper.GetMesssage(EventType.TurnedOff),
                Type = EventType.TurnedOff,
                Time = DateTime.Now
            }, HttpContext.RequestAborted);

            return new StatusResponse { Status = "Ok" };
        }
    }
}
