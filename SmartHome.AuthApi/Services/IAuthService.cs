using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SmartHome.AuthApi.Services
{
    /// <summary>
    /// Сервис аутентификации
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Аутентифицировать пользователя
        /// </summary>
        /// <param name="login">Логин</param>
        /// <param name="password">Пароль</param>
        /// <param name="scopes">API сервисы</param>
        /// <param name="token">Токен отмены</param>
        /// <returns>Bearer токен</returns>
        Task<string> Authenticate(string login, string password, IEnumerable<string> scopes, CancellationToken token = default);
    }
}
