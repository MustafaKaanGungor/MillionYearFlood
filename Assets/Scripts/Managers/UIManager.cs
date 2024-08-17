using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject gameplayPanel;
    public GameObject mainMenu;


    private void OnEnable() {
        GameManager.OnPauseToggled += ToggleMainMenu;
        GameManager.OnPauseToggled += ToggleGameplayPanel;

    }

    private void OnDisable() {
        GameManager.OnPauseToggled -= ToggleMainMenu;
        GameManager.OnPauseToggled -= ToggleGameplayPanel;

    }

    public void ToggleGameplayPanel(bool active) {
        gameplayPanel.gameObject.SetActive(!active);
    }

    public void ToggleMainMenu(bool active) {
        mainMenu.SetActive(active);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
