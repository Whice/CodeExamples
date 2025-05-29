using System;

namespace Utility
{
    /// <summary>
    /// Создан для упрощения выдачи сообщений в консоль,
    /// а также формирования своих.
    /// </summary>
    public abstract class MonoBehaviourLogger : MonoBehaviourAdditionals, ILogger
    {
        private ILogger logger = new SimpleLogger();

        public string logPrefix => logger.logPrefix;
        public void SetLogPrefix(string prefix)
        {
            logger.SetLogPrefix(prefix);
        }
        /// <summary>
        /// Вывести сообщение об ошибке в консоль.
        /// </summary>
        /// <param name="message"></param>
        public void LogError(String message)
        {
            logger.LogError(message);
        }
        /// <summary>
        /// Вывести сообщение об ошибке в консоль.
        /// </summary>
        /// <param name="message"></param>
        public void LogError(object message)
        {
            logger.LogError(message);
        }
        /// <summary>
        /// Вывести предупреждене в консоль.
        /// </summary>
        /// <param name="message"></param>
        public void LogWarning(String message)
        {
            logger.LogWarning(message);
        }
        /// <summary>
        /// Вывести предупреждене в консоль.
        /// </summary>
        /// <param name="message"></param>
        public void LogWarning(object message)
        {
            logger.LogWarning(message);
        }
        /// <summary>
        /// Вывести сообщение в консоль.
        /// </summary>
        /// <param name="message"></param>
        public void LogInfo(String message)
        {
            logger.LogInfo(message);
        }
        /// <summary>
        /// Вывести сообщение в консоль.
        /// </summary>
        /// <param name="message"></param>
        public void LogInfo(object message)
        {
            logger.LogInfo(message);
        }

        /// <summary>
        /// Проверить ссылку на объект на присутсвие.
        /// <br/>Если ссылка нулевая, то выведется ошибка с именем объекта, которое было указано.
        /// </summary>
        /// <param name="checkableObject"></param>
        /// <param name="objectName"></param>
        /// <param name="isError">По умолчания - ошибка. Установить false, если не надо выводить ошибку в консоль.</param>
        /// <returns>Если null, то true.</returns>
        public bool IsNullCheck(object checkableObject, string objectName, bool isError = true)
        {
            return logger.IsNullCheck(checkableObject, objectName, isError);
        }
    }
}