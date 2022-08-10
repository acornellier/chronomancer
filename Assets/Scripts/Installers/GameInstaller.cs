using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<Player>().FromComponentInHierarchy().AsSingle();
        Container.Bind<LevelLoader>().FromComponentInHierarchy().AsSingle();

        Container.BindInterfacesAndSelfTo<GameManager>().AsSingle();

        Container.BindInterfacesAndSelfTo<AudioManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<MenuManager>().AsSingle();
    }
}