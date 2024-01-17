using Fusion;
using UnityEngine;

namespace MainGame
{
    public class NetworkInputHandler : NetworkRunnerCallbacks
    {
        public override void OnInput(NetworkRunner runner, NetworkInput input)
        {
            var data = new PlayerInputData();

            if (Input.GetKey(KeyCode.W))
                data.MoveDirection += Vector2.up;

            if (Input.GetKey(KeyCode.S))
                data.MoveDirection += Vector2.down;

            if (Input.GetKey(KeyCode.A))
                data.MoveDirection += Vector2.left;

            if (Input.GetKey(KeyCode.D))
                data.MoveDirection += Vector2.right;

            if (Input.GetMouseButton(0))
            {
                var mouse_pos = Input.mousePosition;
                var angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
                data.RotateDirection = Quaternion.Euler(0, 0, angle);

                data.NeedToRotate = true;
            }

            input.Set(data);
        }
    }
}
