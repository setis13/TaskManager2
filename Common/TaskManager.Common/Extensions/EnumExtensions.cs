using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace TaskManager.Common.Extensions {
    public static class EnumExtensions {
        /// <summary>
        ///     Gets description from attribute </summary>
        /// <param name="element">Enum element</param>
        /// <returns>Description text</returns>
        public static string GetDescription(this Enum element) {
            Type type = element.GetType();

            MemberInfo[] memberInfo = type.GetMember(element.ToString());
            if (memberInfo.Length > 0) {
                object[] attributes = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attributes.Length > 0) {
                    return ((DescriptionAttribute)attributes[0]).Description;
                }
            }
            return null;
        }

        /// <summary>
        ///     Returns descriptions of enum </summary>
        /// <typeparam name="T">Type of enum</typeparam>
        /// <returns>Dictionary [int of Enum, Description]</returns>
        public static Dictionary<int, string> EnumToDictionary<T>() {
            return typeof(T).GetEnumValues().Cast<Enum>().ToDictionary(Convert.ToInt32, GetDescription);
        }
    }
}
