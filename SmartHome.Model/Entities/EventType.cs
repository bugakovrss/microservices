namespace SmartHome.Model.Entities
{
    /// <summary>
    /// Тип события
    /// </summary>
    public enum EventType
    {
        /// <summary>
        /// Включено
        /// </summary>
        TurnedOn = 1,

        /// <summary>
        /// Выключено
        /// </summary>
        TurnedOff,

        /// <summary>
        /// Ошибка
        /// </summary>
        Error
    }
}
