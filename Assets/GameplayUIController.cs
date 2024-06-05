using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUIController : MonoBehaviour
{
    public Button Pause;
    public Button Resume;
    public Button MainMenu;

    public GameObject PauseCanvas;
    public GameObject GameplayCanvas;
    public MainMenuController MainMenuCanvas;

    private void OnEnable()
    {
        PauseCanvas.SetActive(false);
        GameplayCanvas.SetActive(true);
    }

    private void Start()
    {
        Pause.onClick.AddListener(OnPauseClicked);
        Resume.onClick.AddListener(OnResumeClicked);
        MainMenu.onClick.AddListener(OnMainMenuClicked);
    }

    private void OnPauseClicked()
    {
        Time.timeScale = 0.05f;
        GameplayCanvas?.SetActive(false);
        PauseCanvas?.SetActive(true);
    }

    private void OnResumeClicked()
    {
        Time.timeScale = 1.0f;
        GameplayCanvas?.SetActive(true);
        PauseCanvas?.SetActive(false);
    }

    private void OnMainMenuClicked()
    {
        gameObject.SetActive(false);
        MainMenuCanvas.OpenMainMenu();
    }

    private void OnDestroy()
    {
        Pause?.onClick.RemoveListener(OnPauseClicked);
        Resume?.onClick.RemoveListener(OnResumeClicked);
        MainMenu?.onClick.RemoveListener(OnMainMenuClicked);
    }
}
