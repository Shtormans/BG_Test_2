using Fusion;
using UnityEngine;
using Zenject;

namespace MainGame
{
    public class AnimatedSkin : NetworkBehaviour
    {
        [SerializeField] private Entity _entity;
        [SerializeField] private Sprite _icon;
        [SerializeField] private int _skinId;

        private SpriteRenderer _spriteRenderer;
        private Animator _animator;
        private Vector3 _lastPosition;

        public Sprite Icon => _icon;
        public int SkinId => _skinId;

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

            if (direction.x != 0)
            {
                var yRotation = direction.x > 0 ? 0 : 180;
                _spriteRenderer.transform.eulerAngles = new Vector3(0, yRotation, 0);
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
