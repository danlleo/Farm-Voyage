using System;

namespace Utilities
{
    public static class ArrayExtensions
    {
        public static bool IsNullOrEmpty<T>(T[] array)
        {
            return array == null || array.Length == 0;
        }
        
        public static T GetRandomItem<T>(this T[] array)
        {
            if (IsNullOrEmpty(array))
                throw new ArgumentException("List is empty or null.");
            
            var random = new Random();
            int index = random.Next(array.Length);
            T randomItem = array[index];

            return randomItem;
        }
    }
}