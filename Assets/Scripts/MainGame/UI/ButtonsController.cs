using MainMenu;
using Multiscene;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class ButtonsController : MonoBehaviour
{
    [Inject]
    private NetworkManager _networkManager;

    public void ExitToMainMenu()
    {
        _networkManager.Runner.Shutdown();
        SceneManager.LoadScene((int)SceneIndeces.MainMenu);
    }
}
