using Fusion;
using MainGame;
using UnityEngine;
using Zenject;

public class ContainersInstaller : MonoInstaller
{
    [SerializeField] private PlayersContainer _playersContainer;
    [SerializeField] private EnemyContainer _enemyContainer;

    public override void InstallBindings()
    {
        Container
            .Bind<PlayersContainer>()
            .FromInstance(_playersContainer)
            .AsSingle();
        Container
            .Bind<EnemyContainer>()
            .FromInstance(_enemyContainer)
            .AsSingle();

        Container
            .BindFactory<PlayerBehaviour, PlayerBehaviour, PlayerBehaviour.Factory>()
            .FromFactory<PlayerBehaviour.PlayerBehaviourFactory>();
        Container
            .BindFactory<Weapon, Weapon, Weapon.Factory>()
            .FromFactory<Weapon.WeaponFactory>();
        Container
            .BindFactory<AnimatedSkin, AnimatedSkin, AnimatedSkin.Factory>()
            .FromFactory<AnimatedSkin.AnimatedSkinFactory>();
        Container
            .BindFactory<EnemyController, Vector3, NetworkRunner, PlayerRef, EnemyController, EnemyController.Factory>()
            .FromFactory<EnemyController.EnemyControllerFactory>();
    }
}
