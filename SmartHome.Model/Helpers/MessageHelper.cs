using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using SmartHome.Model.Entities;

namespace SmartHome.Model.Helpers
{
    public static class MessageHelper
    {
        public static string GetMesssage(EventType eventType)
        {
            switch (eventType)
            {
                case EventType.TurnedOn:
                    return "Устройство включено";
                case EventType.TurnedOff:
                    return "Устройство выключено";
                case EventType.Error:
                    return "Ошибка в работе устройства";
                default:
                    throw new ArgumentOutOfRangeException(nameof(eventType), eventType, null);
            }
        }
    }
}
