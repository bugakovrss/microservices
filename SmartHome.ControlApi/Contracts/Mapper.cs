using SmartHome.ControlApi.Services.Model;
using SmartHome.Model.Entities;

namespace SmartHome.ControlApi.Contracts
{
    internal static class Mapper
    {
        public static DeviceModel Map(Device device)
        {
            if (device == null)
                return null;

            return new DeviceModel
            {
                Id = device.Id,
                Name = device.Name,
                State = device.State,
                Description = device.Description
            };
        }

        public static Device Map(DeviceModel device)
        {
            if (device == null)
                return null;

            return new Device
            {
                Id = device.Id,
                Name = device.Name,
                State = device.State,
                Description = device.Description
            };
        }

        public static DeviceEventModel Map(DeviceEventData deviceEvent)
        {
            return new DeviceEventModel
            {
                Id = deviceEvent.Id,
                DeviceId = deviceEvent.DeviceId,
                Time = deviceEvent.Time,
                Type = deviceEvent.Type,
                Message = deviceEvent.Message
            };
        }
    }
}
