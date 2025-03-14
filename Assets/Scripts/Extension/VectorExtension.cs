using UnityEngine;

namespace Extension
{
    public static class VectorExtension
    {
        public static Vector2 ToNearestDirection(this Vector2 v)
        {
            // Normalize the input vector
            v.Normalize();

            // Calculate the angle of the input vector in radians
            var angle = Mathf.Atan2(v.y, v.x);

            // Convert the angle to degrees
            var angleDegrees = angle * Mathf.Rad2Deg;

            // Round the angle to the nearest 45 degrees
            var roundedAngleDegrees = Mathf.Round(angleDegrees / 45) * 45;

            // Convert the rounded angle back to radians
            var roundedAngleRadians = roundedAngleDegrees * Mathf.Deg2Rad;

            // Calculate the closest 45-degree vector
            Vector2 closestVector = new Vector2(Mathf.Cos(roundedAngleRadians), Mathf.Sin(roundedAngleRadians));

            return closestVector;
        }
    }
}
