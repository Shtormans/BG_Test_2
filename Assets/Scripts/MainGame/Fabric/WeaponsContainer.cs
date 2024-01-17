using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    public class WeaponsContainer : MonoBehaviour
    {
        [SerializeField] private List<Weapon> _weaponList;

        public Weapon TakeUniqueWeapon()
        {
            int index = Random.Range(0, _weaponList.Count - 1);
            var weapon = _weaponList[index];

            _weaponList.RemoveAt(index);
            
            return weapon;
        }
    }
}
