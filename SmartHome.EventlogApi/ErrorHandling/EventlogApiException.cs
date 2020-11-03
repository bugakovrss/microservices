using SmartHome.Model.Errors;
using System;

namespace SmartHome.EventlogApi.ErrorHandling
{
    public class EventlogApiException: Exception
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
        public EventlogApiException(string message, ErrorCode code) : base(message)
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
