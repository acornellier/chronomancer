using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    [SerializeField] GameObject essentials;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<GameManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<AudioManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<MenuManager>().AsSingle();
        Container.Bind<LevelLoader>().FromComponentInHierarchy().AsSingle();

        Container.InstantiatePrefab(essentials);
    }
}