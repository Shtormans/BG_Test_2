using MainGame;
using UnityEngine;

namespace MainMenu
{
    public class ToRoomTransfer : MonoBehaviour
    {
        [SerializeField] private AnimatedSkin _defaultSkin;

        private RoomItems _roomItems;

        private void Awake()
        {
            _roomItems = new RoomItems()
            {
                Skin = _defaultSkin
            };
        }

        public RoomItems GetRoomItems()
        {
            return _roomItems;
        }

        public void ChangeRoomItems(RoomItems roomItems)
        {
            _roomItems = roomItems;
        }
    }
}
