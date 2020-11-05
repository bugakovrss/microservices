using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartHome.AuthApi.Contracts;
using SmartHome.AuthApi.Services;
using SmartHome.Model.Errors;

namespace SmartHome.AuthApi.Controllers
{
    /// <summary>
    /// Управление устройствами
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AuthenticateController: ControllerBase
    {
        private readonly IAuthService _authService;

        /// <summary>
        /// Конструткор
        /// </summary>
        /// <param name="authService">Сервис атентификации</param>
        public AuthenticateController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Аутентифицировать пользователя
        /// </summary>
        /// <param name="request">Запрос аутентификации</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(AuthenticateResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.BadRequest)]
        public async Task<AuthenticateResponse> Authenticate([Required][FromBody] AuthenticateRequest request)
        {
            string token = await _authService.Authenticate(request.Login, request.Password,
                request.Scopes,
                HttpContext.RequestAborted);

            return new AuthenticateResponse {Bearer = token};
        }
    }
}
