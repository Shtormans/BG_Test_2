using Fusion;
using UnityEngine;

namespace MainGame
{
    public class AnimatedSkin : NetworkBehaviour
    {
        [SerializeField] private Entity _entity;
        private SpriteRenderer _spriteRenderer;

        private Animator _animator;
        private Vector3 _lastPosition;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }


        private void Update()
        {
            var direction = (_entity.transform.position - _lastPosition).normalized;

            if (direction.x != 0 && _spriteRenderer.flipX != direction.x < 0)
            {
                _spriteRenderer.flipX = !_spriteRenderer.flipX;
            }

            _lastPosition = _entity.transform.position;
        }

        public void SetEntity(Entity entity)
        {
            _entity = entity;

            _lastPosition = _entity.transform.position;
            _entity.Hit += OnHit;
            _entity.Died += OnDied;
        }

        private void OnDied()
        {
            _animator.SetTrigger(AnimaionTriggers.Died);
        }

        private void OnHit()
        {
            _animator.SetTrigger(AnimaionTriggers.Hit);
        }
    }
}
