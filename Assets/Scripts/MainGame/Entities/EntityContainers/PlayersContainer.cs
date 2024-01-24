using Fusion;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject.SpaceFighter;

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
            _players.Add(player);
            PlayerSpawned?.Invoke(player);
        }

        public void SynchroniseAndGetGameResults(Action<List<PlayerGameResult>> action)
        {
            RPC_SynchroniseGameResultsWithHost();
            DataSynchronised += action;
        }

        public bool HasPlayer(PlayerBehaviour player)
        {
            foreach (var item in _players)
            {
                if (player == item)
                {
                    return true;
                }
            }

            return false;
        }

        public PlayerBehaviour GetPlayerById(NetworkBehaviourId id)
        {
            foreach (var item in _players)
            {
                if (id == item.Id)
                {
                    return item;
                }
            }

            return null;
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

            _players.Remove(player);

            if (!HasStateAuthority)
            {
                return;
            }

            _gameStats[player.Id] = new PlayerGameResult()
            {
                Skin = player.Skin,
                Damage = player.GameStats.DamageAmount,
                Kills = player.GameStats.KillsAmount
            };
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
            if (HasStateAuthority)
            {
                UpdateGetGameResults();
                int index = 0;

                foreach (var item in _gameStats)
                {
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
                    DataSynchronised?.Invoke(GetGameResults());
                }
            }
        }
    }
}
