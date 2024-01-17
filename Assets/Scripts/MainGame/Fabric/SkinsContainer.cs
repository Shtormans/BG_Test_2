using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    public class SkinsContainer : MonoBehaviour
    {
        [SerializeField] private List<AnimatedSkin> _skinsList;

        public AnimatedSkin TakeUniqueSkin()
        {
            int index = Random.Range(0, _skinsList.Count - 1);
            var skin = _skinsList[index];

            _skinsList.RemoveAt(index);

            return skin;
        }
    }
}
