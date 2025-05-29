using UnityEngine;

namespace Utility
{
    /// <summary>
    /// Создан для упрощения выдачи сообщений в консоль в любых классах, даже модели,
    /// а также формирования своих.
    /// </summary>
    public sealed class SimpleLogger : ILogger
    {
        public string logPrefix { get; private set; } = "";
        public void SetLogPrefix(string prefix)
        {
            logPrefix = prefix;
        }
        private string AddPrefix(string message)
        {
            if (logPrefix == "" || logPrefix == null)
            {
                return message;
            }
            else
            {
                return $"{logPrefix}> {message}";
            }
        }
        public void LogError(string message)
        {
            Debug.LogError(AddPrefix(message));
        }
        public void LogError(object message)
        {
            Debug.LogError(AddPrefix(message.ToString()));
        }
        public void LogWarning(string message)
        {
            Debug.LogWarning(AddPrefix(message));
        }
        public void LogWarning(object message)
        {
            Debug.LogWarning(AddPrefix(message.ToString()));
        }
        public void LogInfo(string message)
        {
            Debug.Log(AddPrefix(message));
        }
        public void LogInfo(object message)
        {
            Debug.Log(AddPrefix(message.ToString()));
        }

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
