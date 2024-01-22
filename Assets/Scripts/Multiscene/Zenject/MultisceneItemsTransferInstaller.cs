using UnityEngine;
using Zenject;

public class MultisceneItemsTransferInstaller : MonoInstaller
{
    [SerializeField] private MultisceneItemsTransfer _multisceneItemsTransfer;

    public override void InstallBindings()
    {
        Container
            .Bind<MultisceneItemsTransfer>()
            .FromInstance(_multisceneItemsTransfer)
            .AsSingle();
    }
}
