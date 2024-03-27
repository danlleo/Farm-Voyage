using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

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

        public static T GetRandomItemExcept<T>(this IEnumerable<T> array, T exception)
        {
            return array.GetRandomItemExcept(new[] { exception });
        }

        public static T GetRandomItemExcept<T>(this IEnumerable<T> array, IEnumerable<T> exceptions)
        {
            T[] filteredItems = array.Where(item => !exceptions.Contains(item, EqualityComparer<T>.Default)).ToArray();
        
            if (filteredItems.Length == 0)
            {
                Debug.Log("No items available after excluding the exceptions.");
                return default;
            }

            int randomIndex = UnityEngine.Random.Range(0, filteredItems.Length);

            return filteredItems[randomIndex];
        }
    }
}