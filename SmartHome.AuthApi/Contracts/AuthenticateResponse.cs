namespace SmartHome.AuthApi.Contracts
{
    public class AuthenticateResponse
    {
        /// <summary>
        /// Токен доступа
        /// </summary>
        public string Bearer { get; set; }
    }
}
