using UnityEngine;
using UnityEngine.UI;

namespace MainGame
{
    public class GameOverUIManager : MonoBehaviour
    {
        [SerializeField] private VerticalLayoutGroup _tableBody;
        [SerializeField] private PlayersContainer _playersContainer;
        [SerializeField] private GameOverResultRow _resultRow;

        private void OnEnable()
        {
            for (int i = 0; i < _tableBody.transform.childCount; i++)
            {
                Destroy(_tableBody.transform.GetChild(i).gameObject);
            }

            foreach (var gameResult in _playersContainer.GetGameResults())
            {
                var resultRow = Instantiate(_resultRow, _tableBody.transform);
                resultRow.SetUIElements(gameResult);
            }
        }
    }
}
