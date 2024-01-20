using UnityEngine;
using UnityEngine.UIElements;

namespace MainGame
{
    public class JoystickInputHandler : IInputHandler
    {
        private Joystick _movementJoystick;
        private Joystick _rotationJoystick;

        public JoystickInputHandler(Joystick movementJoystick, Joystick rotationJoystick)
        {
            _movementJoystick = movementJoystick;
            _rotationJoystick = rotationJoystick;
        }

        public Vector2 GetDirection()
        {
            var direction = new Vector2(_movementJoystick.Horizontal, _movementJoystick.Vertical);

            return direction;
        }

        public Quaternion GetRotation()
        {
            var angle = Mathf.Atan2(_rotationJoystick.Vertical, _rotationJoystick.Horizontal) * Mathf.Rad2Deg;
            var rotation = Quaternion.Euler(0, 0, angle);

            return rotation;
        }
    }
}
