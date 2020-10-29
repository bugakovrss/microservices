using SmartHome.Model.Entities;

namespace SmartHome.ControlApi.Contracts
{
    /// <summary>
    /// Устройство
    /// </summary>
    public class DeviceModel
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Название 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Состояние
        /// </summary>
        public DeviceState State { get; set; }
    }
}
