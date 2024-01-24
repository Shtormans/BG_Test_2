using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MainGame
{
    public class GameOverUIManager : MonoBehaviour
    {
        [SerializeField] private VerticalLayoutGroup _tableBody;
        [SerializeField] private PlayersContainer _playersContainer;
        [SerializeField] private GameOverResultRow _resultRow;
        [SerializeField] private SkinsContainer _skinsContainer;

        private void OnEnable()
        {
            for (int i = 0; i < _tableBody.transform.childCount; i++)
            {
                Destroy(_tableBody.transform.GetChild(i).gameObject);
            }

            if (_playersContainer.HasStateAuthority)
            {
                UpdateResultsTable(_playersContainer.UpdateGetGameResults());
                _playersContainer.SynchroniseAndGetGameResults(UpdateResultsTable);
            }
            else
            {
                _playersContainer.SynchroniseAndGetGameResults(UpdateResultsTable);
            }
        }

        private void UpdateResultsTable(List<PlayerGameResult> result)
        {
            foreach (var gameResult in result)
            {
                var resultRow = Instantiate(_resultRow, _tableBody.transform);
                resultRow.SetUIElements(gameResult);
            }
        }
    }
}
