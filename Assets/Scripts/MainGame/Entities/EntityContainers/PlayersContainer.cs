using Fusion;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MainGame
{
    public class PlayersContainer : NetworkBehaviour
    {
        [SerializeField] private SkinsContainer _skinsContainer;
        private List<PlayerBehaviour> _players = new();
        private Dictionary<NetworkBehaviourId, PlayerGameResult> _gameStats = new();

        public int PlayersCount => _players.Count;
        public PlayerBehaviour FirstPlayer => _players[0];

        public event Action<PlayerBehaviour> PlayerSpawned;
        private event Action<List<PlayerGameResult>> DataSynchronised;

        public void AddPlayer(PlayerBehaviour player)
        {
            Debug.Log("Adding player");
            _players.Add(player);

            PlayerSpawned?.Invoke(player);
        }

        public void SynchroniseAndGetGameResults(Action<List<PlayerGameResult>> action)
        {
            UpdateGetGameResults();
            Debug.Log("Before start of synchronisation");
            RPC_SynchroniseGameResultsWithHost();
            DataSynchronised += action;
        }

        public List<PlayerGameResult> UpdateGetGameResults()
        {
            _players
                .ForEach(player =>
                {
                    _gameStats[player.Id] = new PlayerGameResult()
                    {
                        Skin = player.Skin,
                        Damage = player.GameStats.DamageAmount,
                        Kills = player.GameStats.KillsAmount
                    };
                });

            return GetGameResults();
        }

        public List<PlayerGameResult> GetGameResults()
        {
            return _gameStats.Values.ToList();
        }

        public void RemovePlayer(PlayerBehaviour player)
        {
            if (!_players.Contains(player))
            {
                return;
            }

            Debug.Log("Before");
            _gameStats[player.Id] = new PlayerGameResult()
            {
                Skin = player.Skin,
                Damage = player.GameStats.DamageAmount,
                Kills = player.GameStats.KillsAmount
            };
            Debug.Log("After");

            _players.Remove(player);
        }

        public PlayerBehaviour GetNearestPlayer(Transform value)
        {
            if (_players.Count == 0)
            {
                return null;
            }

            float minDistance = Vector3.Distance(_players[0].transform.position, value.position);
            int index = 0;

            for (int i = 1; i < _players.Count; i++)
            {
                var distance = Vector3.Distance(_players[i].transform.position, value.position);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    index = i;
                }
            }

            return _players[index];
        }

        [Rpc(RpcSources.All, RpcTargets.All)]
        private void RPC_SynchroniseGameResultsWithHost()
        {
            Debug.Log("Before synchronization");
            if (HasStateAuthority)
            {
                UpdateGetGameResults();
                int index = 0;

                foreach (var item in _gameStats)
                {
                    Debug.Log(item.Value.Damage);
                    NetworkBehaviourId playerId = item.Key;
                    int skinId = item.Value.Skin.SkinId;
                    int damage = item.Value.Damage;
                    int kills = item.Value.Kills;
                    bool isLastDataPackage = index == _gameStats.Count - 1;

                    RPC_GetGameResults(playerId, skinId, damage, kills, isLastDataPackage);
                    index++;
                }
            }
        }

        [Rpc(RpcSources.All, RpcTargets.All)]
        private void RPC_GetGameResults(NetworkBehaviourId playerId, int skinId, int damage, int kills, bool isLastDataPacakge)
        {
            Debug.Log("Random pos");
            if (!HasStateAuthority)
            {
                _gameStats[playerId] = new PlayerGameResult()
                {
                    Skin = _skinsContainer.TakeSkinById(skinId),
                    Damage = damage,
                    Kills = kills
                };

                if (isLastDataPacakge)
                {
                    Debug.Log("Synchronised");
                    DataSynchronised?.Invoke(GetGameResults());
                }
            }
        }
    }
}
