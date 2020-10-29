namespace SmartHome.Model.Entities
{
    /// <summary>
    /// Устройство
    /// </summary>
    public class Device: BaseEntity
    {
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
