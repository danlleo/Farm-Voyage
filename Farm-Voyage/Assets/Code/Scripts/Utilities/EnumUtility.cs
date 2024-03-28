using System;

namespace Utilities
{
    public class EnumUtility
    {
        private static readonly Random s_random = new();

        public static T GetRandomEnumValue<T>() where T : Enum
        {
            Array values = Enum.GetValues(typeof(T));
            int index = s_random.Next(values.Length);
            return (T)values.GetValue(index);
        }
    }
}