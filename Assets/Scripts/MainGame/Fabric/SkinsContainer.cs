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

        public AnimatedSkin TakeSkinById(int id)
        {
            foreach (var item in _skinsList)
            {
                if (item.SkinId == id)
                {
                    return item;
                }
            }

            return null;
        }
    }
}
