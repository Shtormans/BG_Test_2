using Fusion;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public class SessionListUIItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _playersCountText;
        [SerializeField] private Button _joinButton;

        private SessionInfo _sessionInfo;
        public event Action<SessionInfo> SessionJoined;

        public void SetSessionInfo(SessionInfo sessionInfo)
        {
            _sessionInfo = sessionInfo;

            _nameText.text = sessionInfo.Name;
            _playersCountText.text = ConvertSessionToAmountOfPlayersString(sessionInfo);

            _joinButton.enabled = sessionInfo.PlayerCount < sessionInfo.MaxPlayers;
        }

        private string ConvertSessionToAmountOfPlayersString(SessionInfo sessionInfo)
        {
            return $"{sessionInfo.PlayerCount}/{sessionInfo.MaxPlayers}";
        }

        public void JoinSession()
        {
            SessionJoined?.Invoke(_sessionInfo);
        }
    }
}
