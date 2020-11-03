using System;
using SmartHome.Model.Entities;

namespace SmartHome.ControlApi.Services.Model
{
    public class DeviceEventData
    {
        public long Id { get; set; }

        /// <summary>
        /// Тип события
        /// </summary>
        public EventType Type { get; set; }

        /// <summary>
        /// Сообщение
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Время события
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// Устройство
        /// </summary>
        public long DeviceId { get; set; }
    }
}
