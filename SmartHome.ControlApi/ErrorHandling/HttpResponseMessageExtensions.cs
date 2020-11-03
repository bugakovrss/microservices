using System.Net.Http;
using System.Threading.Tasks;
using SmartHome.Model.Errors;
using SmartHome.Net;

namespace SmartHome.ControlApi.ErrorHandling
{
    public static class HttpResponseMessageExtensions
    {
        public static async Task<ControlApiException> ReadErrorAsync(this HttpResponseMessage message)
        {
            var error = await message.ContentAsAsync<ErrorModel>();
            return new ControlApiException(error.Error, error.ErrorCode);
        }
    }
}
