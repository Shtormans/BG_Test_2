using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    public class PlayersContainer : MonoBehaviour
    {
        [SerializeField] private List<PlayerBehaviour> _players;

        private static PlayersContainer _instance;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
        }

        public void AddPlayer(PlayerBehaviour player)
        {
            _instance._players.Add(player);
        }

        public PlayerBehaviour GetNearestPlayer(Transform value)
        {
            float minDistance = Vector3.Distance(_instance._players[0].transform.position, value.position);
            int index = 0;

            for (int i = 1; i < _instance._players.Count; i++)
            {
                var distance = Vector3.Distance(_instance._players[i].transform.position, value.position);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    index = i;
                }
            }

            return _instance._players[index];
        }
    }
}
