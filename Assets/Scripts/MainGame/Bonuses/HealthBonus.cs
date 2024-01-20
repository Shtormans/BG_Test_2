using UnityEngine;

namespace MainGame
{
    public class HealthBonus : BonusController
    {
        [SerializeField] private int _healthBonus;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out PlayerBehaviour player))
            {
                PickUp(player);
            }
        }

        public override void Affect(PlayerBehaviour player)
        {
            player.AddHealthBonus(_healthBonus);
        }
    }
}
