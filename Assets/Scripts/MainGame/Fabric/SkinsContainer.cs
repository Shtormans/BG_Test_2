using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace MainGame
{
    public class SkinsContainer : MonoBehaviour
    {
        [SerializeField] private List<AnimatedSkin> _skinsList;

        [Inject]
        private MultisceneItemsTransfer _multisceneItemsTransfer;

        public AnimatedSkin TakeSkin()
        {
            return _multisceneItemsTransfer.GetMultisceneItems().Skin;
        }
    }
}
