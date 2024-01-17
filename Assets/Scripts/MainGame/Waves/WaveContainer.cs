using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    public class WaveContainer : MonoBehaviour
    {
        [SerializeField] private List<Wave> _waves;
        private int _currentIndex;

        private void OnEnable()
        {
            _currentIndex = -1;
        }

        public Wave Current()
        {
            return _waves[_currentIndex];
        }

        public Wave NextWave()
        {
            _currentIndex++;
            return _waves[_currentIndex];
        }
    }
}
