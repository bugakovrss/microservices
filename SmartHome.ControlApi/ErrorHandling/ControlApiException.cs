using System;
using SmartHome.Model.Errors;

namespace SmartHome.ControlApi.ErrorHandling
{
    public class ControlApiException: Exception
    {
        /// <summary>
        /// Код ошибки
        /// </summary>
        public ErrorCode ErrorCode { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="message">Сообщение об ошибке</param>
        /// <param name="code">Код ошибки</param>
        public ControlApiException(string message, ErrorCode code) : base(message)
        {
            ErrorCode = code;
        }

        /// <summary>
        /// Получить ошибку
        /// </summary>
        /// <returns></returns>
        public ErrorModel GetError()
        {
            return new ErrorModel { ErrorCode = ErrorCode, Error = Message };
        }
    }
}
