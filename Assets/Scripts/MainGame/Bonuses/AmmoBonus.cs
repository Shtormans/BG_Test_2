using UnityEngine;

namespace MainGame
{
    public class AmmoBonus : BonusController
    {
        [SerializeField] private int _clipsAmount;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out PlayerBehaviour player))
            {
                PickUp(player);
            }
        }

        public override void Affect(PlayerBehaviour player)
        {
            player.PlayerWeapon.AddClips(_clipsAmount);
        }
    }
}
