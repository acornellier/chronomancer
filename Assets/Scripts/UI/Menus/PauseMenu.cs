using UnityEngine;
using Zenject;

public class PauseMenu : Menu
{
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject pauseMenuUi;

    [Inject] GameManager _gameManager;

    PlayerInputActions _playerControls;

    void Awake()
    {
        _playerControls = new PlayerInputActions();
    }

    void OnEnable()
    {
        _playerControls.Player.Pause.Enable();
        _playerControls.Player.Pause.performed += (_) => OnPauseInput();
        _gameManager.OnGamePauseChange += OnGamePauseChange;
    }

    void OnDisable()
    {
        _playerControls.Player.Pause.Disable();
        _gameManager.OnGamePauseChange -= OnGamePauseChange;
    }

    void OnPauseInput()
    {
        if (_gameManager.State != GameState.Paused)
            _gameManager.SetState(GameState.Paused);
    }

    void OnGamePauseChange(bool paused)
    {
        if (paused) PauseCallback();
        else ResumeCallback();
    }

    void PauseCallback()
    {
        menuManager.CloseAll();
        pausePanel.SetActive(true);
        menuManager.OpenMenu(pauseMenuUi);
    }

    void ResumeCallback()
    {
        menuManager.CloseAll();
        pausePanel.SetActive(false);
    }

    // public void MainMenu()
    // {
    //     SceneManager.LoadScene("StartMenu");
    // }

    public void Quit()
    {
        GameUtilities.Quit();
    }
}