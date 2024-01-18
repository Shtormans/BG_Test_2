using Cinemachine;
using Fusion;
using System;
using UnityEngine;

namespace MainGame
{
    public class PlayerBehaviour : Entity
    {
        [SerializeField] private Transform _body;
        private PlayerMover _playerPhysics;

        [Networked] public PlayerBody PlayerBody { get; set; }

        public Transform Body => _body;
        public event Action<PlayerBehaviour> SpawnedEvent;

        public override void Spawned()
        {
            _playerPhysics = GetComponent<PlayerMover>();
            new PlayersContainer().AddPlayer(this);

            SpawnedEvent?.Invoke(this);
        }

        public override void FixedUpdateNetwork()
        {
            if (!HasStateAuthority)
            {
                return;
            }

            if (GetInput(out PlayerInputData data))
            {
                _playerPhysics.Move(Stats.EntityData.Speed, data.MoveDirection, Runner.DeltaTime);


                if (data.NeedToRotate)
                {
                    Weapon.transform.rotation = data.RotateDirection;
                    Weapon.Shoot();
                }
            }
        }

        public void SetPlayerData(PlayerBody playerBody)
        {
            PlayerBody = playerBody;
        }

        public void SetWeapon(Weapon weapon)
        {
            Weapon = weapon;
        }

        public void SetCamera(CinemachineVirtualCamera camera)
        {
            if (!Object.HasInputAuthority)
            {
                return;
            }

            camera.Follow = transform;
        }
    }
}
