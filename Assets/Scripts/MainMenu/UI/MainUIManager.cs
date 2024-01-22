using UnityEngine;

namespace MainMenu
{
    public class MainUIManager : MonoBehaviour
    {
        [SerializeField] private Canvas _mainMenu;
        [SerializeField] private Canvas _skinMenu;
        [SerializeField] private Canvas _roomMenu;

        private Canvas _activeCanvas;

        private void OnEnable()
        {
            _activeCanvas = _mainMenu;
        }

        public void ToRoomMenu()
        {
            ChangeCanvas(_roomMenu);
        }

        public void ToMainMenu()
        {
            ChangeCanvas(_mainMenu);
        }

        public void ToSkinMenu()
        {
            ChangeCanvas(_skinMenu);
        }

        public void ExitFromGame()
        {
            Application.Quit();
        }

        private void ChangeCanvas(Canvas newCanvas)
        {
            _activeCanvas.gameObject.SetActive(false);
            
            _activeCanvas = newCanvas;
            _activeCanvas.gameObject.SetActive(true);
        }
    }
}
