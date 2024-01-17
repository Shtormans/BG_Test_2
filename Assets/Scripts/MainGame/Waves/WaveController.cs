using UnityEngine;

namespace MainGame
{
    public class WaveController : MonoBehaviour
    {
        [SerializeField] private WaveContainer _waveContainer;

        private EnemyFabric _enemyFabric;
        private BonusesFabric _bonusFabric;

        private void Awake()
        {
            _enemyFabric = new EnemyFabric();
            _bonusFabric = new BonusesFabric();
        }

        public void SpawnNewWave()
        {
            var wave = _waveContainer.NextWave();

            _enemyFabric.CreateWave(wave);
            _bonusFabric.CreateWave(wave);
        }
    }
}
