using UnityEditor;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Collider2D))]
public class LevelEnd : MonoBehaviour
{
    [SerializeField] SceneAsset nextScene;

    [Inject] LevelLoader _levelLoader;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<Player>())
            _levelLoader.LoadScene(nextScene.name);
    }
}