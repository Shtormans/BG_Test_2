using Fusion;
using System;
using UnityEngine;

namespace MainGame
{
    public class PlayerBehaviour : Entity
    {
        [SerializeField] private Weapon _weapon;

        private PlayerMover _playerPhysics;

        private void Awake()
        {
            _playerPhysics = GetComponent<PlayerMover>();
            new PlayersContainer().AddPlayer(this);
        }

        public override void FixedUpdateNetwork()
        {
            if (GetInput(out PlayerInputData data))
            {
                _playerPhysics.Move(data.MoveDirection, Runner.DeltaTime);

                if (data.NeedToRotate)
                {
                    _weapon.transform.rotation = data.RotateDirection;
                    _weapon.Shoot();
                }
            }
        }
    }
}
