using System;
using UnityEngine;

namespace Utility
{
    /// <summary>
    /// Допольнительные возможности для наследников <see cref="MonoBehaviour"/>
    /// </summary>
    public abstract class MonoBehaviourAdditionals : MonoBehaviour
    {
        /// <summary>
        /// Объект прошёл этап уничтожения.
        /// </summary>
        public bool isDestroyed { get; protected set; } = false;
        /// <summary>
        /// После этого события измениться состояние объекта "вкл/выкл".
        /// </summary>
        public event Action<bool> preActiveStateChanged;
        /// <summary>
        /// Изменилось состояние объекта "вкл/выкл".
        /// </summary>
        public event Action<bool> activeStateChanged;
        /// <summary>
        /// Установить активность для <see cref="GameObject"/> этого компонента.
        /// </summary>
        public void SetActiveObject(bool isActive)
        {
            if (isDestroyed)
            {
                Debug.LogError("An attempt to change the activity of a destroyed object!");
            }
            else
            {
                preActiveStateChanged?.Invoke(isActive);
                gameObject.SetActive(isActive);
                activeStateChanged?.Invoke(isActive);
            }
        }
        /// <summary>
        /// Установить активность для <see cref="GameObject"/> этого компонента.
        /// </summary>
        protected void SetActiveObject(Component component, bool isActive)
        {
            if (component is MonoBehaviourAdditionals additionals)
            {
                additionals.SetActiveObject(isActive);
            }
            else
            {
                component.gameObject.SetActive(isActive);
            }
        }
        /// <summary>
        /// Удалить текущего родителя для <see cref="Transform"/> этого объекта,
        /// а потом назначить его обратно.
        /// Это полезно, когда нужно переместить этот дочерний объект в конец списка всех детей.
        /// </summary>
        public void ResetParent()
        {
            Transform parent = transform.parent;
            transform.SetParent(null, false);
            transform.SetParent(parent, false);
        }
        protected virtual void OnDestroy()
        {
            isDestroyed = true;
        }
    }
}