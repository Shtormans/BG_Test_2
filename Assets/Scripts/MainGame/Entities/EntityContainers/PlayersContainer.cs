using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MainGame
{
    public class PlayersContainer : MonoBehaviour
    {
        private List<PlayerBehaviour> _players;
        private Dictionary<PlayerBehaviour, PlayerGameResult> _gameStats;

        public int PlayersCount => _players.Count;
        public PlayerBehaviour FirstPlayer => _players[0];

        public event Action<PlayerBehaviour> PlayerSpawned;

        private void Awake()
        {
            _players = new();
        }

        public void AddPlayer(PlayerBehaviour player)
        {
            _players.Add(player);
            _gameStats[player] = new PlayerGameResult()
            {
                Icon = player.Skin.Icon,
                Damage = player.GameStats.DamageAmount,
                Kills = player.GameStats.KillsAmount
            };

            PlayerSpawned?.Invoke(player);
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

            _gameStats[player].Damage = player.GameStats.DamageAmount;
            _gameStats[player].Kills = player.GameStats.KillsAmount;

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
    }
}
