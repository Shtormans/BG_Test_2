using Fusion;
using Multiscene;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace MainMenu
{
    public class NetworkRoomController : NetworkRunnerCallbacks
    {
        private List<SessionInfo> _sessionList;
        private NetworkRunner _runner;
        private const int _playerCount = 2;
        private const string _lobbyName = "Lobby";
        private const string _netwrokRunnerObjectName = "NetworkRunner";

        public IReadOnlyList<SessionInfo> SessionList => _sessionList;
        public event Action<IReadOnlyList<SessionInfo>> SessionListWasUpdated;

        [Inject]
        private MultisceneItemsTransfer _multisceneItemsTransfer;

        private void Awake()
        {
            _sessionList = new List<SessionInfo>();
        }

        public override void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
        {
            _sessionList = sessionList;
            SessionListWasUpdated?.Invoke(sessionList);
        }

        public void CreateRoom(string roomName)
        {
            StartSession(roomName, GameMode.Host);
        }

        public void JoinRoom(string roomName)
        {
            StartSession(roomName, GameMode.Client);
        }

        public void JoinLobby()
        {
            StartLobby();
        }

        public void ExitLobby()
        {
            _runner?.Shutdown();
        }

        private async void StartLobby()
        {
            _runner = new GameObject() 
            { 
                name = _netwrokRunnerObjectName 
            }.AddComponent<NetworkRunner>();
            _runner.ProvideInput = true;

            var roomItems = _multisceneItemsTransfer.GetMultisceneItems();
            roomItems.NetworkRunner = _runner;
            _multisceneItemsTransfer.ChangeMultisceneItems(roomItems);

            await _runner.JoinSessionLobby(SessionLobby.Custom, _lobbyName);
        }

        private async void StartSession(string roomName, GameMode mode)
        {
            var scene = SceneRef.FromIndex((int)SceneIndeces.Game);
            var sceneInfo = new NetworkSceneInfo();
            if (scene.IsValid)
            {
                sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
            }

            await _runner.StartGame(new StartGameArgs()
            {
                GameMode = mode,
                SessionName = roomName,
                Scene = scene,
                PlayerCount = _playerCount,
                SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
            });
        }
    }
}
