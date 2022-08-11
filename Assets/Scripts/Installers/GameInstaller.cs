using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] bool fadeInLights = true;

    [Inject] LevelLoader _levelLoader;

    public override void InstallBindings()
    {
        Container.Bind<Player>().FromComponentInHierarchy().AsSingle();
    }

    public void Awake()
    {
        if (fadeInLights)
            _levelLoader.FadeInLights();
    }
}