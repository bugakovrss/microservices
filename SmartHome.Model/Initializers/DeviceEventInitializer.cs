using System;
using System.Collections.Generic;
using System.Linq;
using SmartHome.Model.Entities;
using SmartHome.Model.Helpers;

namespace SmartHome.Model.Initializers
{
    public static class DeviceEventInitializer
    {
        public static IReadOnlyCollection<DeviceEvent> Initialize()
        {
            var list = new List<DeviceEvent>(); 
            for (int i = 0; i < 5; i++)
            {

                InitEvents(list, i, 10); 
            }

            return list;
        }


        private static void InitEvents(List<DeviceEvent> events, long deviceId, int maxCount)
        {
            var random = new Random();
            long startId = events.Any() ? events.Max(x => x.DeviceId) + 1 : 1;

            int count = random.Next(1, maxCount);

            for (long i = startId; i < startId + count; i++)
            {
                var eventType = (EventType) random.Next(1, (int) EventType.Error);

                events.Add(new DeviceEvent
                {
                    Id = i,
                    DeviceId = deviceId,
                    Time = DateTime.UtcNow.AddDays(- random.Next(maxCount - (int)i)),
                    Type = eventType,
                    Message = MessageHelper.GetMesssage(eventType)
                });
            }
        }
    }
}
