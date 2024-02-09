using UnityEngine;

namespace Utilities
{
    public static class ColliderExtensions
    {
        /// <summary>
        /// Enables the specified collider.
        /// </summary>
        /// <param name="collider">The collider to be enabled.</param>
        public static void Enable(this Collider collider)
        {
            collider.enabled = true;
        }

        /// <summary>
        /// Disables the specified collider.
        /// </summary>
        /// <param name="collider">The collider to be disabled.</param>
        public static void Disable(this Collider collider)
        {
            collider.enabled = false;
        }
    }
}