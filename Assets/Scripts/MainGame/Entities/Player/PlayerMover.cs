using UnityEngine;

namespace MainGame
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMover : MonoBehaviour
    {
        [SerializeField] private float _speed = 10f;

        private Rigidbody2D _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        public void Move(Vector2 direction, float deltaTime)
        {
            var newPosition = transform.position + (Vector3)(_speed * deltaTime * direction);
            _rigidbody.MovePosition(newPosition);
        }
    }
}
