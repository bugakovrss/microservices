using System.Collections.Generic;
using SmartHome.Model.Entities;

namespace SmartHome.Model.Initializers
{
    public static class DeviceInitializer
    {
        public static IReadOnlyCollection<Device> Initialize()
        {
            return new List<Device>
            {
                new Device()
                {
                    Id = 1,
                    Name = "Выключатель на кухне",
                    State = DeviceState.Off
                },
                new Device()
                {
                    Id = 2,
                    Name = "Выключатель в спальне",
                    State = DeviceState.On
                },
                new Device()
                {
                    Id = 3,
                    Name = "TV samsung",
                    State = DeviceState.Off
                },
                new Device()
                {
                    Id = 4,
                    Name = "Ноутбук",
                    State = DeviceState.On
                }
            };
        }
    }
}
