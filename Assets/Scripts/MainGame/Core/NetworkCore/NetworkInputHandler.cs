using Fusion;
using UnityEngine;

namespace MainGame
{
    public class NetworkInputHandler : NetworkRunnerCallbacks
    {
        [SerializeField] private FixedJoystick _movementJoystick;
        [SerializeField] private FixedJoystick _rotationJoystick;
        private IInputHandler _inputHandler;

        private void Awake()
        {
#if UNITY_EDITOR
            SetInputHandler(new KeyboardMouseInputHandler());
#elif UNITY_ANDROID
            SetInputHandler(new JoystickInputHandler(_movementJoystick, _rotationJoystick));
#endif
        }

        private void SetInputHandler(IInputHandler inputHandler)
        {
            _inputHandler = inputHandler;
        }

        public override void OnInput(NetworkRunner runner, NetworkInput input)
        {
            var data = new PlayerInputData
            {
                MoveDirection = _inputHandler.GetDirection(),
                RotationDirection = _inputHandler.GetRotation()
            };

            input.Set(data);
        }
    }
}
