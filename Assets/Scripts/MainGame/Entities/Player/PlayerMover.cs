using UnityEngine;

namespace MainGame
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMover : MonoBehaviour
    {
        private Rigidbody2D _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        public void Move(float speed, Vector2 direction, float deltaTime)
        {
            var newPosition = transform.position + (Vector3)(speed * deltaTime * direction);
            _rigidbody.MovePosition(newPosition);
        }
    }
}
