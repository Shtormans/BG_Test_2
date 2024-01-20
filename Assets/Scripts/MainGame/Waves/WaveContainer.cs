using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    public class WaveContainer : MonoBehaviour
    {
        [SerializeField] private List<Wave> _waves;
        private int _currentIndex;

        public Wave CurrentWave => _waves[_currentIndex];
        public bool HasNextWave => _currentIndex < _waves.Count;

        private void OnEnable()
        {
            _currentIndex = -1;
        }

        public Wave MoveToNextWave()
        {
            _currentIndex++;
            return _waves[_currentIndex];
        }
    }
}
