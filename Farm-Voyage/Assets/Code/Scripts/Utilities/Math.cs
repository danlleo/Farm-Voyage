using UnityEngine;

namespace Utilities
{
    public static class Math
    {
        /// <summary>
        /// Maps a value from one range to another
        /// </summary>
        /// <param name="iMin">Minimum threshold</param>
        /// <param name="iMax">Maximum threshold</param>
        /// <param name="oMin">Minimum out value</param>
        /// <param name="oMax">Maximum out value</param>
        /// <param name="value">Depending value</param>
        /// <returns></returns>
        public static Color RemapColor(float iMin, float iMax, Color oMin, Color oMax, float value)
        {
            float t = Mathf.InverseLerp(iMin, iMax, value);
            Color outColor = Color.Lerp(oMin, oMax, t);

            return outColor;
        }
        
        public static void RemapTransform(float iMin, float iMax, Transform oMin, Transform oMax, float value, Transform outputTransform)
        {
            float t = Mathf.InverseLerp(iMin, iMax, value);

            outputTransform.position = Vector3.Lerp(oMin.position, oMax.position, t);
            outputTransform.rotation = Quaternion.Lerp(oMin.rotation, oMax.rotation, t);
            outputTransform.localScale = Vector3.Lerp(oMin.localScale, oMax.localScale, t);
        }

        /// <summary>
        /// Calculates the normalized position of value 'v' within the range [a, b].
        /// </summary>
        /// <param name="a">The start of the range.</param>
        /// <param name="b">The end of the range.</param>
        /// <param name="v">The value to find the normalized position for.</param>
        /// <returns>A normalized value between 0 and 1 indicating 'v's position within the range. Returns 0 if 'a' equals 'b'.</returns>
        public static float InverseLerp(float a, float b, float v)
        {
            if (a == b) return 0;
            
            return (v - a) / (b - a);
        }
    }
}
