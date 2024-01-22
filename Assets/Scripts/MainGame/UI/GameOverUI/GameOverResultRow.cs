using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MainGame
{
    public class GameOverResultRow : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _amountOfKillsText;
        [SerializeField] private TextMeshProUGUI _amountOfDamageText;

        public void SetUIElements(PlayerGameResult gameOverResult)
        {
            _icon.sprite = gameOverResult.Icon;
            _amountOfKillsText.text = gameOverResult.Kills.ToString();
            _amountOfDamageText.text = gameOverResult.Damage.ToString();
        }
    }
}
