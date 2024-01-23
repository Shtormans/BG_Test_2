using UnityEngine;
using Zenject;

namespace MainGame
{
    public class PlayerInjectionManager : MonoBehaviour
    {
        [Inject] private PlayerBehaviour.Factory _playerBehaviourFactory;
        [Inject] private Weapon.Factory _weaponFactory;
        [Inject] private AnimatedSkin.Factory _skinFactory;

        private static PlayerInjectionManager _instance;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
        }

        public static void InjectIntoPlayer(PlayerBehaviour player)
        {
            _instance._playerBehaviourFactory.Create(player);
        }

        public static void InjectIntoWeapon(Weapon weapon)
        {
            _instance._weaponFactory.Create(weapon);
        }

        public static void InjectIntoSkin(AnimatedSkin skin)
        {
            _instance._skinFactory.Create(skin);
        }
    }
}
