using Fusion;
using System;
using UnityEngine;

namespace MainGame
{
    public abstract class BonusController : NetworkBehaviour
    {
        private bool _alreadyInUse = false;
        public event Action<BonusController> PickedUp;

        protected void PickUp(PlayerBehaviour player)
        {
            if (!HasStateAuthority || _alreadyInUse)
            {
                return;
            }

            _alreadyInUse = true;
            Affect(player);

            PickedUp?.Invoke(this);

            Runner.Despawn(Object);
        }

        public abstract void Affect(PlayerBehaviour player);
    }
}
