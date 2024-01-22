using Multiscene;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class ButtonsController : MonoBehaviour
{
    [Inject]
    private MultisceneItemsTransfer _mulisceneItemsTransfer;

    public void ExitToMainMenu()
    {
        _mulisceneItemsTransfer.GetMultisceneItems().NetworkRunner.Shutdown();
        SceneManager.LoadScene((int)SceneIndeces.MainMenu);
    }
}
