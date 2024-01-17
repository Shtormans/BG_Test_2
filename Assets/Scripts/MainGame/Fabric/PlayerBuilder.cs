﻿using Cinemachine;
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
            weapon.transform.parent = _player.transform;
            weapon.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            _player.SetWeapon(weapon);

            return this;
        }

        public PlayerBuilder AddSkin(AnimatedSkin skin)
        {
            skin.SetEntity(_player);
            skin.transform.parent = _player.transform;
            skin.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

            return this;
        }

        public PlayerBuilder AddCamera(CinemachineVirtualCamera camera)
        {
            camera.Follow = _player.transform;

            return this;
        }

        public PlayerBuilder AddJoysticks()
        {
            return this;
        }

        public PlayerBehaviour Build()
        {
            return _player;
        }
    }
}