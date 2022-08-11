using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    [SerializeField] GameObject essentials;

    [SerializeField] Texture2D mouseCursor;
    [SerializeField] Vector2 hotSpot;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<GameManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<AudioManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<MenuManager>().AsSingle();
        Container.Bind<LevelLoader>().FromComponentInHierarchy().AsSingle();

        Container.InstantiatePrefab(essentials);

        Cursor.SetCursor(mouseCursor, hotSpot, CursorMode.Auto);
    }
}