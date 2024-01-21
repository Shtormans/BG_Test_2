using Cinemachine;
using Fusion;
using System;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

namespace MainGame
{
    public class PlayerBehaviour : Entity
    {
        [SerializeField] private Transform _body;
        private PlayerMover _playerPhysics;
        private PlayerGameStats _gameStats;

        [Networked] public PlayerBody PlayerBody { get; set; }

        public Transform Body => _body;
        public WeaponWithBullets PlayerWeapon => Weapon as WeaponWithBullets;
        public PlayerGameStats GameStats => _gameStats;
        public event Action<PlayerBehaviour> SpawnedEvent;
        public event Action Killed;

        public override void Spawned()
        {
            Init();

            _playerPhysics = GetComponent<PlayerMover>();
            new PlayersContainer().AddPlayer(this);

            SpawnedEvent?.Invoke(this);
        }

        public override void FixedUpdateNetwork()
        {

            if (!GetInput(out PlayerInputData data))
            {
                return;
            }

            if (HasStateAuthority)
            {
                UseInputOnHost(data);
            }

            UseInputOnClients(data);
        }

        public void AddHealthBonus(int healthBonus)
        {
            AddHealth(healthBonus);
        }

        protected override void Died()
        {
            Runner.Spawn(DeadCopy,
                        transform.position,
                        Quaternion.identity,
                        Object.InputAuthority);
        }

        private void UseInputOnHost(PlayerInputData inputData)
        {
            if (!inputData.MoveDirection.IsZero())
            {
                _playerPhysics.Move(Stats.EntityData.Speed, inputData.MoveDirection, Runner.DeltaTime);

                if (!IsRunning)
                {
                    RPC_SendTrigger(AnimationTriggers.StartedRunning);
                }
            }
            else
            {
                if (IsRunning)
                {
                    RPC_SendTrigger(AnimationTriggers.StoppedRunning);
                }
            }


            if (!inputData.RotationDirection.IsZero())
            {
                Weapon.Rotate(inputData.RotationDirection);
            }
        }

        private void UseInputOnClients(PlayerInputData inputData)
        {
            if (!inputData.RotationDirection.IsZero())
            {
                Weapon.Shoot();
            }
        }

        public void SetPlayerData(PlayerBody playerBody)
        {
            PlayerBody = playerBody;
        }

        public void SetWeapon(Weapon weapon)
        {
            Weapon = weapon;
            weapon.Hit += OnWeaponHit;
        }

        public void SetCamera(CinemachineVirtualCamera camera)
        {
            camera.Follow = transform;
        }

        public void OnWeaponHit(EntityHitStatus hitStatus)
        {
            _gameStats.DamageAmount += hitStatus.Damage;

            if (hitStatus.Died)
            {
                _gameStats.KillsAmount++;
                RPC_HitEvent();
            }
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.InputAuthority)]
        private void RPC_HitEvent(RpcInfo info = default)
        {
            Killed?.Invoke();
        }
    }
}
