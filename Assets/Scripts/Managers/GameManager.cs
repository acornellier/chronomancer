using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class GameManager : ITickable
{
    public void Tick()
    {
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}