using SmartHome.Model.Entities;

namespace SmartHome.EventlogApi.Contracts
{
    public static class Mapper
    {
        public static DeviceEventModel Map(DeviceEvent deviceEvent)
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

        public static DeviceEvent Map(DeviceEventModel deviceEvent)
        {
            return new DeviceEvent
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
