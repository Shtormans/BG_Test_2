using Fusion;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MainMenu
{
    public class RoomsManager : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _roomNameInput;
        [SerializeField] private Button _roomCreateButton;
        [SerializeField] private VerticalLayoutGroup _layoutGroup;
        [SerializeField] private SessionListUIItem _uiSession;

        private int _minAmountOfLettersInRoomName = 3;

        [Inject] private NetworkManager _roomsController;

        private void OnEnable()
        {
            _roomsController.JoinLobby();
            _roomsController.SessionListWasUpdated += UpdateTable;
        }

        private void OnDisable()
        {
            _roomsController.SessionListWasUpdated -= UpdateTable;
        }

        public void ExitLobby()
        {
            _roomsController.ExitLobby();
        }

        public void OnRoomNameValueChanged()
        {
            if (!CheckForRoomNameLength(_roomNameInput.text))
            {
                return;
            }

            if (!CheckIfAlreadyExist(_roomNameInput.text))
            {
                return;
            }

            _roomCreateButton.interactable = true;
        }

        public void CreateRoom()
        {
            _roomsController.CreateRoom(_roomNameInput.text);
        }

        public void UpdateTable()
        {
            if (!_roomNameInput.interactable)
            {
                _roomNameInput.interactable = true;
            }

            for (int i = 0; i < _layoutGroup.transform.childCount; i++)
            {
                Destroy(_layoutGroup.transform.GetChild(i).gameObject);
            }

            foreach (var session in _roomsController.SessionList)
            {
                var uiSession = Instantiate(_uiSession, _layoutGroup.transform);
                uiSession.SetSessionInfo(session);
                uiSession.ChoseRoom += JoinRoom;
            }
        }

        public void JoinRoom(SessionInfo session)
        {
            _roomsController.JoinRoom(session.Name);
        }

        private bool CheckForRoomNameLength(string roomName)
        {
            if (roomName.Length < _minAmountOfLettersInRoomName)
            {
                _roomCreateButton.interactable = false;
                return false;
            }

            return true;
        }

        private bool CheckIfAlreadyExist(string roomName)
        {
            bool alreadyExist = _roomsController
                .SessionList
                .Select(session => session.Name)
                .Any(room => room.Equals(roomName));

            if (alreadyExist)
            {
                _roomCreateButton.interactable = false;
                return false;
            }

            return true;
        }
    }
}
