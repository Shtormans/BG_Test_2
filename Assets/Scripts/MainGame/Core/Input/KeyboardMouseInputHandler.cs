using UnityEngine;

namespace MainGame
{
    public class KeyboardMouseInputHandler : IInputHandler
    {
        public Vector2 GetDirection()
        {
            var direction = Vector2.zero;

            if (Input.GetKey(KeyCode.W))
                direction += Vector2.up;

            if (Input.GetKey(KeyCode.S))
                direction += Vector2.down;

            if (Input.GetKey(KeyCode.A))
                direction += Vector2.left;

            if (Input.GetKey(KeyCode.D))
                direction += Vector2.right;

            return direction;
        }

        public Quaternion GetRotation()
        {
            var rotation = Quaternion.identity;

            if (Input.GetMouseButton(0))
            {
                var mousePosition = Input.mousePosition;
                var positionX = mousePosition.x - Screen.width / 2f;
                var positionY = mousePosition.y - Screen.height / 2f;

                var angle = Mathf.Atan2(positionY, positionX) * Mathf.Rad2Deg;
                rotation = Quaternion.Euler(0, 0, angle);
            }

            return rotation;
        }
    }
}
