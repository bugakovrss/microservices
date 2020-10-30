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
    }
}
