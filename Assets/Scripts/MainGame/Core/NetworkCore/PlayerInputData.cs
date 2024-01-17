using Fusion;
using UnityEngine;

namespace MainGame
{
    public struct PlayerInputData : INetworkInput
    {
        public Vector2 MoveDirection;

        public Quaternion RotateDirection;
        public bool NeedToRotate;
    }
}
