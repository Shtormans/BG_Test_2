using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public class RoomsManager : MonoBehaviour
    {
        [SerializeField] private InputField _roomNameInput;
        [SerializeField] private TextMeshProUGUI _errorText;
        [SerializeField] private SessionsListManager _sessionsManager;

        private int _minAmountOfLettersInRoomName;

        public void CreateRoom()
        {
            if (CheckForRoomNameLength(_roomNameInput.text))
            {
                return;
            }

            if (CheckIfAlreadyExist(_roomNameInput.text))
            {
                return;
            }


        }

        private bool CheckForRoomNameLength(string roomName)
        {
            if (roomName.Length < _minAmountOfLettersInRoomName)
            {
                _errorText.text = $"Room name is too small";
                return false;
            }

            return true;
        }

        private bool CheckIfAlreadyExist(string roomName)
        {
            bool alreadyExist = _sessionsManager
                .GetSessionNames()
                .Any(room => room.Equals(roomName));

            if (alreadyExist)
            {
                _errorText.text = "Room with such name already exist";
                return false;
            }

            return true;
        }
    }
}
