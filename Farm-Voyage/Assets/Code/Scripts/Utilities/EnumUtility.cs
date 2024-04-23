using System;
using System.Collections.Concurrent;
using System.Security.Cryptography;

namespace Utilities
{
    public static class EnumUtility
    {
        private static readonly ConcurrentDictionary<Type, Array> s_enumValuesCache = new();
        private static readonly RandomNumberGenerator s_rng = RandomNumberGenerator.Create();

        public static T GetRandomEnumValue<T>() where T : Enum
        {
            Array values = s_enumValuesCache.GetOrAdd(typeof(T), t => Enum.GetValues(t));

            byte[] buffer = new byte[4];
            s_rng.GetBytes(buffer);
            int index = Math.Abs(BitConverter.ToInt32(buffer, 0)) % values.Length;

            return (T)values.GetValue(index);
        }
    }
}