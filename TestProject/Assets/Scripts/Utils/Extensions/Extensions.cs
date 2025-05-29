using System;
using System.Text;
using UnityEngine;

namespace Utility
{
    public static class Extensions
    {
        //string
        /// <summary>
        /// Удалить все пробелы в строке.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string RemoveAllSpaces(this string text)
        {
            return text.Replace(" ", "");
        }
        /// <summary>
        /// Заменить все запятые на точки.
        /// </summary>
        public static string ReplaceAllDots(this string text)
        {
            return text.Replace(".", ",");
        }
        /// <summary>
        /// Строка заполнена. Т.е. является значимой.
        /// <br/>Это означает, что она не null, не пустая, не состоит только из пробелов.
        /// </summary>
        public static bool IsFill(this string text)
        {
            return !string.IsNullOrEmpty(text) && !string.IsNullOrWhiteSpace(text);
        }
        /// <summary>
        /// Текст является типом числа <see cref="Decimal"/>.
        /// </summary>
        public static bool IsDecimal(this string text)
        {
            return decimal.TryParse(text.ReplaceAllDots(), out decimal decimalValue);
        }

        /// <summary>
        /// В строке, которая является числом, расставить пробелы.
        /// </summary>
        /// <param name="strNumber"></param>
        /// <param name="isNegative">Число отрицательное.</param>
        /// <returns></returns>
        private static string ToStringWithSpaces(string strNumber)
        {
            StringBuilder sb = new StringBuilder(strNumber.Length + 1);
            int counterDigits = strNumber.Length % 3;
            foreach (char c in strNumber)
            {
                if (counterDigits == 0)
                {
                    sb.Append(" ");
                    counterDigits = 3;
                }
                sb.Append(c);
                --counterDigits;
            }

            return sb.ToString();
        }
        /// <summary>
        /// Попытаться преобразовать текст в Int32.
        /// </summary>
        /// <param name="text">Текст для преобразования.</param>
        /// <param name="value">Полученное значение, если преобразование возможно.</param>
        /// <returns>true, если текст можно преобразовать в Int32.</returns>
        public static bool TryParseToInt32(this string text, out int value)
        {
            return Int32.TryParse(text.RemoveAllSpaces(), out value);
        }
        /// <summary>
        /// Попытаться преобразовать текст в float.
        /// </summary>
        /// <param name="text">Текст для преобразования.</param>
        /// <param name="value">Полученное значение, если преобразование возможно.</param>
        /// <returns>true, если текст можно преобразовать в float.</returns>
        public static bool TryParseToFloat(this string text, out float value)
        {
            return float.TryParse(text.RemoveAllSpaces(), out value);
        }

        //UInt32
        /// <summary>
        /// Получить строковую запись числа с пробелами между каждыми 3 цифрами.
        /// Нужно для облегчения чтения.
        /// </summary>
        public static string ToStringWithSpaces(this UInt32 number)
        {
            string strNumber = number.ToString();
            return ToStringWithSpaces(strNumber);
        }


        //Int32
        /// <summary>
        /// Получить строковую запись числа с пробелами между каждыми 3 цифрами.
        /// Нужно для облегчения чтения.
        /// </summary>
        public static string ToStringWithSpaces(this int number)
        {
            return ToStringWithSpaces(number.ToString());
        }

        //Decimal
        /// <summary>
        /// Получить строковую запись числа с пробелами между каждыми 3 цифрами.
        /// Нужно для облегчения чтения.
        /// </summary>
        public static string ToStringWithSpaces(this Decimal number)
        {
            string[] parts = number.ToString().Split('.');
            string integer = parts[0];
            string fractional = parts.Length > 1 ? parts[1] : "";
            return ToStringWithSpaces(integer.ToString()) + fractional;
        }

        //Animator
        public static bool TryGetFirstAnimatorBehviour<T>(this Animator animator, out T behaviour) where T : StateMachineBehaviour
        {
            behaviour = null;
            T[] behaviours = animator.GetBehaviours<T>();

            if (behaviours.Length != 0)
            {
                behaviour = behaviours[0];
            }

            return behaviour != null;
        }


        //Vector3
        /// <summary>
        /// Конвертация в 2D вектор по горизонтальной плоскости движка.
        /// <br/>x = x, y = z;
        /// </summary>
        public static Vector2 ToVector2Horizontal(this Vector3 v3)
        {
            return new Vector2(v3.x, v3.z);
        }
        //Vector2
        /// <summary>
        /// Если результат больше 0, то точка 2 в левой полуполскости.
        /// 0 - на тойже линии.
        /// Меньше в правой полуполскости.
        /// </summary>
        public static float Skew(this Vector2 v1, Vector2 v2)
        {
            return (v1.x * v2.y) - (v1.y * v2.x);
        }

        //Transform
        /// <summary>
        /// Цель (точка) находится слева от объекта по его направлению (<see cref="Transform.forward"/>).
        /// <br/> Если объект направлен прямо в цель, т.е. он ни справа и ни слева, то это не учитывается
        /// и считается, что он справа.
        /// </summary>
        /// <param name="me">Объект - точка отсчета.</param>
        /// <param name="target">Цель, которая будет слева или справа.</param>
        /// <returns></returns>
        public static bool IsTargetLeft(this Transform me, in Vector3 target)
        {
            Vector2 forward2D = me.forward.ToVector2Horizontal();
            Vector2 hitVectort2D = (target - me.transform.position).ToVector2Horizontal();

            bool isLeft = forward2D.Skew(hitVectort2D) > 0;
            return isLeft;
        }
    }
}