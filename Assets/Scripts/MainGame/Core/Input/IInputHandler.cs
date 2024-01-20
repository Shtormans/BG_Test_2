using UnityEngine;

namespace MainGame
{
    public interface IInputHandler
    {
        public Vector2 GetDirection();
        public Quaternion GetRotation();
    }
}
