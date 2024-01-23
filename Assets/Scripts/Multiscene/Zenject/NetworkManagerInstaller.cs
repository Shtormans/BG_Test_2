using MainMenu;
using UnityEngine;
using Zenject;

public class NetworkManagerInstaller : MonoInstaller
{
    [SerializeField] private NetworkManager _networkManager;

    public override void InstallBindings()
    {
        Container
            .Bind<NetworkManager>()
            .FromInstance(_networkManager)
            .AsSingle();
    }
}
