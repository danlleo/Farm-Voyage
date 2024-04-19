using UnityEngine;

namespace Utilities
{
    /// <summary>
    /// Provides utility methods for mathematical operations specific to Unity, such as remapping values to different scales.
    /// </summary>
    public static class MathUtils
    {
        /// <summary>
        /// Remaps a given value from one range to another and interpolates a corresponding Color.
        /// </summary>
        /// <param name="iMin">Input range minimum value.</param>
        /// <param name="iMax">Input range maximum value.</param>
        /// <param name="oMin">Output range minimum Color.</param>
        /// <param name="oMax">Output range maximum Color.</param>
        /// <param name="value">Value to remap.</param>
        /// <returns>Interpolated Color.</returns>
        public static Color RemapColor(float iMin, float iMax, Color oMin, Color oMax, float value)
        {
            float normalizedValue = Mathf.InverseLerp(iMin, iMax, value);
            return Color.Lerp(oMin, oMax, normalizedValue);
        }

        /// <summary>
        /// Remaps a given value from one range to another and interpolates position, rotation, and scale for a Transform.
        /// </summary>
        /// <param name="iMin">Input range minimum value.</param>
        /// <param name="iMax">Input range maximum value.</param>
        /// <param name="oMin">Output range minimum Transform.</param>
        /// <param name="oMax">Output range maximum Transform.</param>
        /// <param name="value">Value to remap.</param>
        /// <param name="outputTransform">Transform to apply the interpolated values to.</param>
        public static void RemapTransform(float iMin, float iMax, Transform oMin, Transform oMax, float value, Transform outputTransform)
        {
            float normalizedValue = Mathf.InverseLerp(iMin, iMax, value);
            outputTransform.position = Vector3.Lerp(oMin.position, oMax.position, normalizedValue);
            outputTransform.rotation = Quaternion.Lerp(oMin.rotation, oMax.rotation, normalizedValue);
            outputTransform.localScale = Vector3.Lerp(oMin.localScale, oMax.localScale, normalizedValue);
        }

        /// <summary>
        /// Calculates the normalized position of a value within a specified range.
        /// </summary>
        /// <param name="a">Start of the range.</param>
        /// <param name="b">End of the range.</param>
        /// <param name="v">Value to normalize within the range.</param>
        /// <returns>Normalized value between 0 and 1, or 0 if the range start equals the range end.</returns>
        public static float InverseLerp(float a, float b, float v)
        {
            if (a == b) return 0;
            return (v - a) / (b - a);
        }
    }
}
