using UnityEngine;

namespace MainGame
{
    public class PlayerBehaviour : Entity
    {
        [SerializeField] private Transform _body;
        private PlayerMover _playerPhysics;

        public Transform Body => _body;

        private void Awake()
        {
            _playerPhysics = GetComponent<PlayerMover>();
            new PlayersContainer().AddPlayer(this);
        }

        public override void FixedUpdateNetwork()
        {
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

        public void SetWeapon(Weapon weapon)
        {
            Weapon = weapon;
        }
    }
}
