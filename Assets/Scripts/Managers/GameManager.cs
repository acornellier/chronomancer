using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class GameManager : ITickable
{
    [Inject] LevelLoader _levelLoader;

    public void Tick()
    {
        // if (Input.GetKeyDown(KeyCode.R))
        // _levelLoader.LoadScene(SceneManager.GetActiveScene().name);
    }
}