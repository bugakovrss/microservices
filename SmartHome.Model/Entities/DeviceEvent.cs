using System;

namespace SmartHome.Model.Entities
{
    public class DeviceEvent: BaseEntity
    {
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
