using UnityEngine;

namespace MainGame
{
    public abstract class EnemyController : Entity
    {
        [SerializeField] public float DistanceToShoot { get; protected set; }
        [SerializeField] public float Damage { get; protected set; }
        [SerializeField] public Weapon Weapon { get; protected set; }

        protected abstract void Move();
    }
}