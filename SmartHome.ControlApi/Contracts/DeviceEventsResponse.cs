using System.Collections.Generic;
using SmartHome.Model.Entities;

namespace SmartHome.ControlApi.Contracts
{
    /// <summary>
    /// Ответ на запрос событий
    /// </summary>
    public class DeviceEventsResponse
    {
        /// <summary>
        /// Идентификатор устройства
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Название устройства
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Состояние
        /// </summary>
        public DeviceState State { get; set; }

        /// <summary>
        /// События
        /// </summary>
        public List<DeviceEventModel> Events { get; set; }
    }
}
