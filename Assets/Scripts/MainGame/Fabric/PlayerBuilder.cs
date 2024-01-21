using Cinemachine;
using Fusion;
using System;
using UnityEngine;

namespace MainGame
{
    public class PlayerBuilder
    {
        private PlayerBehaviour _player;

        private PlayerBuilder(PlayerBehaviour player)
        {
            _player = player;
        }

        public static PlayerBuilder CreateBuilder(PlayerBehaviour player)
        {
            return new PlayerBuilder(player);
        }

        public PlayerBuilder AddWeapon(Weapon weapon)
        {
            var networkTransform = weapon.gameObject.GetComponent<NetworkTransform>();
            networkTransform.enabled = false;

            weapon.transform.parent = _player.Body.transform;
            weapon.transform.localPosition = Vector3.zero;
            weapon.transform.rotation = Quaternion.identity;
            _player.SetWeapon(weapon);

            networkTransform.enabled = true;
            
            return this;
        }

        public PlayerBuilder AddSkin(AnimatedSkin skin)
        {
            var networkTransform = skin.gameObject.GetComponent<NetworkTransform>();
            networkTransform.enabled = false;

            skin.SetEntity(_player);
            skin.transform.parent = _player.Body.transform;
            skin.transform.localPosition = Vector3.zero;
            skin.transform.rotation = Quaternion.identity;

            networkTransform.enabled = true;

            return this;
        }

        public PlayerBuilder AddCamera(CinemachineVirtualCamera camera)
        {
            _player.SetCamera(camera);

            return this;
        }

        public PlayerBuilder AddJoysticks()
        {
            return this;
        }

        public PlayerBuilder AddPlayerBody(PlayerBody playerBody)
        {
            _player.SetPlayerData(playerBody);

            return this;
        }

        public PlayerBuilder AddUIController(PlayerStatsUIController playerStatsUIController)
        {
            playerStatsUIController.SetPlayer(_player);

            return this;
        }

        public PlayerBehaviour Build()
        {
            return _player;
        }
    }
}
