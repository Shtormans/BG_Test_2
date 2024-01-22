using MainGame;
using UnityEngine;
using Zenject;

public class PlayersContainerInstaller : MonoInstaller
{
    [SerializeField] private PlayersContainer _playersContainer;

    public override void InstallBindings()
    {
        Container
            .Bind<PlayersContainer>()
            .FromInstance(_playersContainer)
            .AsSingle();

        Container
            .BindFactory<EnemyController, EnemyController.Factory>();
        Container
            .BindFactory<PlayerBehaviour, PlayerBehaviour.Factory>();
    }
}
