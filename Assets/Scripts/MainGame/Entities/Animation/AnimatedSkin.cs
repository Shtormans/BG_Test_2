﻿using Fusion;
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

            if (_entity != null)
            {
                _entity.AnimationTriggered += OnAnimationTriggered;
            }
            _lastPosition = transform.position;
        }


        private void Update()
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
            Debug.Log(transform.parent);
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
