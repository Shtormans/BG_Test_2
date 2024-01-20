using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    public class BonusFabric : MonoBehaviour
    {
        [SerializeField] private NetworkSpawner _networkSpawner;

        public List<BonusController> CreateWave(Wave wave)
        {
            var bonuses = new List<BonusController>();

            foreach (var item in wave.Bonuses)
            {
                for (int i = 0; i < item.Amount; i++)
                {
                    var bonus = Spawn(item.Bonus);
                    bonuses.Add(bonus);
                }
            }

            return bonuses;
        }

        private BonusController Spawn(BonusController bonus)
        {
            return _networkSpawner.Runner.Spawn(bonus, Vector3.zero, Quaternion.identity);
        }
    }
}
