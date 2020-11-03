namespace SmartHome.ControlApi.Configuration
{
    public class EventlogApiEndpoints
    {
        /// <summary>
        /// Хост
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Получить события
        /// </summary>
        public string GetEvents { get; set; }

        /// <summary>
        /// Добавить событие
        /// </summary>
        public string AddEvent { get; set; }
    }
}
