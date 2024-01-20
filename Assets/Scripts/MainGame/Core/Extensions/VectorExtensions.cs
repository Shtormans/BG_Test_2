using UnityEngine;

namespace MainGame
{
    public static class VectorExtensions
    {
        public static bool IsZero(this Quaternion quaternion)
        {
            return quaternion.x == 0 
                && quaternion.y == 0 
                && quaternion.z == 0 
                && quaternion.w == 1;
        }

        public static bool IsZero(this Vector3 vector)
        {
            return vector.x == 0
                && vector.y == 0
                && vector.z == 0;
        }

        public static bool IsZero(this Vector2 vector)
        {
            return vector.x == 0
                && vector.y == 0;
        }
    }
}
