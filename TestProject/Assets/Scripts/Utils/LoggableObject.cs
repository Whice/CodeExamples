using System;
using Utility;

namespace Common
{
    /// <summary>
    /// Объект, предоставляющий методы для логирования.
    /// </summary>
    [Serializable]
    public abstract class LoggableObject
    {
        [NonSerialized] private ILogger logger;
        private void CheckLogger()
        {
            if (logger == null)
            {
                logger = new SimpleLogger();
                LogError("Logger is not set! Setted default logger.");
            }
        }
        public void SetLoggerWithPrefix(ILogger logger, string className = "")
        {
            SetLogger(logger);
            if (className != "")
                SetLogPrefix(className);
        }
        public void SetLogger(ILogger logger)
        {
            this.logger = logger;
        }
        public void SetLogPrefix(string prefix)
        {
            CheckLogger();
            logger.SetLogPrefix(prefix);
        }
        /// <summary>
        /// Вывести сообщение об ошибке в консоль.
        /// </summary>
        /// <param name="message"></param>
        public void LogError(String message)
        {
            CheckLogger();
            logger.LogError(message);
        }
        /// <summary>
        /// Вывести сообщение об ошибке в консоль.
        /// </summary>
        /// <param name="message"></param>
        public void LogError(object message)
        {
            CheckLogger();
            logger.LogError(message);
        }
        /// <summary>
        /// Вывести предупреждене в консоль.
        /// </summary>
        /// <param name="message"></param>
        public void LogWarning(String message)
        {
            CheckLogger();
            logger.LogWarning(message);
        }
        /// <summary>
        /// Вывести предупреждене в консоль.
        /// </summary>
        /// <param name="message"></param>
        public void LogWarning(object message)
        {
            CheckLogger();
            logger.LogWarning(message);
        }
        /// <summary>
        /// Вывести сообщение в консоль.
        /// </summary>
        /// <param name="message"></param>
        public void LogInfo(String message)
        {
            CheckLogger();
            logger.LogInfo(message);
        }
        /// <summary>
        /// Вывести сообщение в консоль.
        /// </summary>
        /// <param name="message"></param>
        public void LogInfo(object message)
        {
            CheckLogger();
            logger.LogInfo(message);
        }

        /// <summary>
        /// Проверить ссылку на объект на присутсвие.
        /// <br/>Если ссылка нулевая, то выведется ошибка с именем объекта, которое было указано.
        /// </summary>
        /// <param name="isError">По умолчания - ошибка. Установить false, если не надо выводить ошибку в консоль.</param>
        /// <returns>Если null, то true.</returns>
        public bool IsNullCheck(object checkableObject, string objectName, bool isError = true)
        {
            if (isError && checkableObject == null)
            {
                LogError($"{objectName} is null!");
            }

            return checkableObject == null;
        }
    }
}
