using UnityEngine;

namespace MainGame
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/WeaponStats", order = 1)]
    public class WeaponStats : ScriptableObject
    {
        public Bullet Bullet;

        public int Damage;
        public float TimeBeforeDisappear;
        public float BulletSpeed;
        public bool InfiniteAmmo;
        public int Clip;
        public int StartAmount;
        public float ReloadTime;
        public float FireRate;
    }
}
