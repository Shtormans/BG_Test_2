using Fusion;
using UnityEngine;

namespace MainGame
{
    public abstract class BonusController : NetworkBehaviour
    {
        private bool _alreadyInUse = false;

        protected void PickUp(PlayerBehaviour player)
        {
            if (!HasStateAuthority || _alreadyInUse)
            {
                return;
            }

            _alreadyInUse = true;
            Affect(player);

            Runner.Despawn(Object);
        }

        public abstract void Affect(PlayerBehaviour player);
    }
}
