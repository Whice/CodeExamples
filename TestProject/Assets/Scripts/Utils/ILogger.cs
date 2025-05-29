using System;

namespace Utility
{
    /// <summary>
    /// Интерфейс для передачи логики логирования.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Префикс добавляемый к сообщениям, если он не пуст.
        /// </summary>
        string logPrefix { get; }
        void SetLogPrefix(string prefix);
        /// <summary>
        /// Вывести сообщение об ошибке в консоль.
        /// </summary>
        /// <param name="message"></param>
        void LogError(String message);
        /// <summary>
        /// Вывести сообщение об ошибке в консоль.
        /// </summary>
        /// <param name="message"></param>
        void LogError(object message);
        /// <summary>
        /// Вывести предупреждене в консоль.
        /// </summary>
        /// <param name="message"></param>
        void LogWarning(String message);
        /// <summary>
        /// Вывести предупреждене в консоль.
        /// </summary>
        /// <param name="message"></param>
        void LogWarning(object message);
        /// <summary>
        /// Вывести сообщение в консоль.
        /// </summary>
        /// <param name="message"></param>
        void LogInfo(String message);
        /// <summary>
        /// Вывести сообщение в консоль.
        /// </summary>
        /// <param name="message"></param>
        void LogInfo(object message);
        /// <summary>
        /// Проверить ссылку на объект на присутсвие.
        /// <br/>Если ссылка нулевая, то выведется ошибка с именем объекта, которое было указано.
        /// </summary>
        /// <param name="checkableObject"></param>
        /// <param name="objectName"></param>
        /// <param name="isError">По умолчания - ошибка. Установить false, если не надо выводить ошибку в консоль.</param>
        /// <returns>Если null, то true.</returns>
        bool IsNullCheck(object checkableObject, string objectName, bool isError = true);
    }
}