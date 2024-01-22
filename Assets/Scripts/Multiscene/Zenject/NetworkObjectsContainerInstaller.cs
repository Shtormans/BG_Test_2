using MainGame;
using UnityEngine;
using Zenject;

public class NetworkObjectsContainerInstaller : MonoInstaller
{
    [SerializeField] private NetworkObjectsContainer _networkObjectsContainer;

    public override void InstallBindings()
    {
        Container
            .Bind<NetworkObjectsContainer>()
            .FromInstance(_networkObjectsContainer)
            .AsSingle();
    }
}
