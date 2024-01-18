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

        public override void Spawned()
        {
            NetworkObjectsContainer.Instance.AddObject(this);
        }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();

            _lastPosition = transform.position;
        }


        private void Update()
        {
            if (_entity == null)
            {
                return;
            }

            var direction = (_entity.transform.position - _lastPosition).normalized;
            SetIsRunning(direction.x != 0 || direction.y != 0);

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
            entity.Hit += OnHit;
            entity.Died += OnDied;
        }

        private void SetIsRunning(bool isRunning)
        {
            _animator.SetFloat(AnimationNameHelpers.Speed, isRunning ? 1 : 0);
        }

        private void OnDied()
        {
            _animator.SetTrigger(AnimationNameHelpers.Died);
        }

        private void OnHit()
        {
            _animator.SetTrigger(AnimationNameHelpers.Hit);
        }
    }
}
