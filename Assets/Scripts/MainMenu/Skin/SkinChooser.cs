using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace MainMenu
{
    public class SkinChooser : MonoBehaviour
    {
        [SerializeField] private List<SkinBlock> _skinBlocks;

        [Inject]
        private MultisceneItemsTransfer _toRoomTransfer;

        private void OnEnable()
        {
            foreach (var block in _skinBlocks)
            {
                block.ButtonClicked += OnSkinButtonClicked;
            }
        }

        private void OnSkinButtonClicked(MainGame.AnimatedSkin skin)
        {
            var roomItems = _toRoomTransfer.GetMultisceneItems();
            roomItems.Skin = skin;
            
            _toRoomTransfer.ChangeMultisceneItems(roomItems);
        }
    }
}
