using Fusion;
using UnityEngine;
using Zenject;

namespace MainGame
{
    public class AnimatedSkin : NetworkBehaviour
    {
        [SerializeField] private Entity _entity;
        [SerializeField] private Sprite _icon;

        private SpriteRenderer _spriteRenderer;
        private Animator _animator;
        private Vector3 _lastPosition;
        public readonly int SkinId = 1;

        public Sprite Icon => _icon;

        public class Factory : PlaceholderFactory<AnimatedSkin, AnimatedSkin>
        {

        }

        public class AnimatedSkinFactory : IFactory<AnimatedSkin, AnimatedSkin>
        {
            private DiContainer _diContainer;

            public AnimatedSkinFactory(DiContainer diContainer)
            {
                _diContainer = diContainer;
            }

            public AnimatedSkin Create(AnimatedSkin prefab)
            {
                _diContainer.Inject(prefab);

                return prefab;
            }
        }

        [Inject]
        public void Construct(NetworkObjectsContainer networkObjectsContainer)
        {
            networkObjectsContainer.AddObject(this);
        }

        public override void Spawned()
        {
            PlayerInjectionManager.InjectIntoSkin(this);
        }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();

            if (_entity != null)
            {
                _entity.AnimationTriggered += OnAnimationTriggered;
            }
            _lastPosition = transform.position;
        }

        public override void FixedUpdateNetwork()
        {
            if (_entity == null)
            {
                return;
            }

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
            entity.AnimationTriggered += OnAnimationTriggered;
        }

        private void OnAnimationTriggered(AnimationTriggers trigger)
        {
            switch (trigger)
            {
                case AnimationTriggers.Hit:
                case AnimationTriggers.Died:
                    _animator.SetTrigger(trigger.ToString());
                    break;
                case AnimationTriggers.StartedRunning:
                    _animator.SetBool(AniationVariables.IsRunning, true);
                    break;
                case AnimationTriggers.StoppedRunning:
                    _animator.SetBool(AniationVariables.IsRunning, false);
                    break;
                default:
                    break;
            }
            
        }
    }
}
