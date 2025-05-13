using DG.Tweening;
using System.Linq;
using UnityEngine;

namespace View
{
    /// <summary>
    /// Анимация для присоединения одного корабля к другому при абордаже.
    /// Один корабль поворачивается, другой не двигается.
    /// </summary>
    public sealed class ShipBoardingConnectionAnimation
    {
        public class Ship
        {
            public Transform shipTransform;
            public Transform[] connectionPoints;
        }
        /// <summary>
        /// Неподвижный корабль.
        /// </summary>
        public Ship staticShip;
        /// <summary>
        /// Корабль, который будет двигаться.
        /// </summary>
        public Ship movingShip;
        /// <summary>
        /// Длительность анимации.
        /// </summary>
        public float connectionDuration = 4f;
        private Sequence _currentConnectionSequence;

        private (Transform, Transform) FindConnectionPoints()
        {
            // Найдем ближайшие точки соединения
            Transform closestMovingPoint = null;
            Transform closestStaticPoint = null;
            float minDistance = float.MaxValue;

            // Перебираем все комбинации точек соединения
            foreach (Transform movingPoint in movingShip.connectionPoints)
            {
                foreach (Transform staticPoint in staticShip.connectionPoints)
                {
                    float distance = Vector3.Distance(movingPoint.position, staticPoint.position);

                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        closestMovingPoint = movingPoint;
                        closestStaticPoint = staticPoint;
                    }
                }
            }

            return (closestMovingPoint, closestStaticPoint);
        }
        public void CancelConnection()
        {
            if (_currentConnectionSequence != null)
            {
                _currentConnectionSequence.Kill();
                _currentConnectionSequence = null;
            }
        }
        /// <summary>
        /// Запустить анимацию соединения кораблей при абордаже.
        /// </summary>
        /// <param name="connectionDuration">Длительность анимации.</param>
        public void ConnectShips(float connectionDuration)
        {
            this.connectionDuration = connectionDuration;

            // Найдем точки соединения
            (Transform movingPoint, Transform staticPoint) = FindConnectionPoints();


            // Вычислим направление поворота
            Vector3 directionToTarget = staticShip.shipTransform.forward;
            if (Vector3.Dot(directionToTarget, movingShip.shipTransform.forward) < 0)
            {
                directionToTarget *= -1;
            }
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget, Vector3.up);

            // Создаем последовательность анимации
            _currentConnectionSequence = DOTween.Sequence();

            // Поворот движущегося корабля
            _currentConnectionSequence.Join(movingShip.shipTransform.DORotateQuaternion(targetRotation, connectionDuration));

            //По направлению точки надо посчитать, насколько далеко должен встать движимый корбаль относительно неё.
            //По сути так две боковые точки соединяться и будут иметь одно положение.
            //Боковые точки должны быть повёрнуты в сторону, и смотреть также, как и пушки, в свой бок.
            Vector3 direction = staticPoint.forward;
            float distance = Mathf.Abs((movingShip.shipTransform.position - movingPoint.position).magnitude);

            // Перемещение движущегося корабля к точке соединения статического корабля
            _currentConnectionSequence.Join(movingShip.shipTransform.DOMove(
                staticPoint.position + (direction * distance),
                connectionDuration
            ));

            _currentConnectionSequence.OnComplete(() =>
            {
                CancelConnection();
            });

            _currentConnectionSequence.Play();
        }
    }
}
