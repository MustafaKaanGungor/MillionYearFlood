using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject gameplayPanel;
    public GameObject mainMenu;
    public GameObject gameOverPanel;

    public TMP_Text[] ResourceTexts;


    private void OnEnable() {
        GameManager.OnPauseToggled += ToggleMainMenu;
        GameManager.OnPauseToggled += ToggleGameplayPanel;
        GameManager.OnGameOver += ToggleGameOverPanel;
    }

    private void OnDisable() {
        GameManager.OnPauseToggled -= ToggleMainMenu;
        GameManager.OnPauseToggled -= ToggleGameplayPanel;
        GameManager.OnGameOver -= ToggleGameOverPanel;
    }

    public void ToggleGameplayPanel(bool active) {
        gameplayPanel.SetActive(!active);
    }

    public void ToggleMainMenu(bool active) {
        mainMenu.SetActive(active);
    }

    public void ToggleGameOverPanel() {
        ToggleGameplayPanel(false);
        gameOverPanel.SetActive(true);
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void ReloadScene() {
        SceneManager.LoadScene(0);
    }
}
