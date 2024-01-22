using Cinemachine;
using Fusion;
using System;
using UnityEngine;
using Zenject;

namespace MainGame
{
    public class PlayerBehaviour : Entity
    {
        [SerializeField] private Transform _body;
        private PlayerMover _playerPhysics;
        private AnimatedSkin _skin;
        private PlayerRPCHandler _rpcHandler;
        private PlayerGameStats _gameStats;

        [Networked] public PlayerBody PlayerBody { get; set; }
        public Transform Body => _body;
        public WeaponWithBullets PlayerWeapon => Weapon as WeaponWithBullets;
        public AnimatedSkin Skin => _skin;
        public PlayerRPCHandler RpcHandler => _rpcHandler;

        public PlayerGameStats GameStats => _gameStats;
        public event Action<PlayerGameStats> Killed;
        public event Action<WeaponWithBullets> WeaponChanged;

        private PlayersContainer _playersContainer;
        private NetworkObjectsContainer _networkObjectsContainer;

        [Inject]
        private void Construct(PlayersContainer playersContainer, NetworkObjectsContainer networkObjectsContainer)
        {
            _playersContainer = playersContainer;
            _networkObjectsContainer = networkObjectsContainer;
        }

        public class Factory : PlaceholderFactory<PlayerBehaviour>
        {
        }

        public override void Spawned()
        {
            Init();

            _playerPhysics = GetComponent<PlayerMover>();
            _rpcHandler = GetComponent<PlayerRPCHandler>();

            _playersContainer.AddPlayer(this);
            _networkObjectsContainer.AddObject(this);
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
            WeaponChanged?.Invoke(PlayerWeapon);
        }

        public void SetSkin(AnimatedSkin skin)
        {
            _skin = skin;
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
                Killed?.Invoke(GameStats);
            }
        }
    }
}
