using Cinemachine;
using Fusion;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    public class PlayerFabric : MonoBehaviour
    {
        [SerializeField] private PlayerBehaviour prefab;
        [SerializeField] private WeaponsContainer _weaponsContainer;
        [SerializeField] private SkinsContainer _skinsContainer;
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;

        public PlayerBehaviour Create(NetworkRunner runner, PlayerRef inputAuthority)
        {
            var player = runner.Spawn(prefab, inputAuthority: inputAuthority);
            var weapon = runner.Spawn(_weaponsContainer.TakeUniqueWeapon());
            var skin = runner.Spawn(_skinsContainer.TakeUniqueSkin());

            player = PlayerBuilder.CreateBuilder(player)
                .AddWeapon(weapon)
                .AddSkin(skin)
                .AddCamera(_virtualCamera)
                .Build();

            return player;
        }
    }
}
