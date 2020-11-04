using System.Collections.Generic;

namespace SmartHome.AuthApi.Contracts
{
    public class AuthenticateRequest
    {
        /// <summary>
        /// Логин
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// API сервисы
        /// </summary>
        public List<string> Scopes { get; set; }
    }
}
