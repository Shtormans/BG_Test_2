using System.Collections.Generic;
using UnityEngine;

namespace MainMenu
{
    public class SkinChooser : MonoBehaviour
    {
        [SerializeField] private List<SkinBlock> _skinBlocks;
        [SerializeField] private ToRoomTransfer _toRoomTransfer;

        private void OnEnable()
        {
            foreach (var block in _skinBlocks)
            {
                block.ButtonClicked += OnSkinButtonClicked;
            }
        }

        private void OnSkinButtonClicked(MainGame.AnimatedSkin skin)
        {
            var roomItems = _toRoomTransfer.GetRoomItems();
            roomItems.Skin = skin;
            
            _toRoomTransfer.ChangeRoomItems(roomItems);
        }
    }
}
