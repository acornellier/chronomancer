using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [Inject] LevelLoader _levelLoader;

    public override void InstallBindings()
    {
        Container.Bind<Player>().FromComponentInHierarchy().AsSingle();
    }

    public void Awake()
    {
        _levelLoader.StartLevel();
    }
}