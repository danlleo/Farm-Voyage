using System;
using System.Linq;
using System.Reflection;

namespace Utilities
{
    public static class TypeUtility
    {
        public static Type[] FindAllDerivedTypes<T>()
        {
            Type derivedType = typeof(T);

            return Assembly
                .GetAssembly(derivedType)
                .GetTypes()
                .Where(t => t != derivedType && derivedType.IsAssignableFrom(t))
                .ToArray();
        }
    }
}