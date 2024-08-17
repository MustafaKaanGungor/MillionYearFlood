using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button waitButton;
    public GameObject mainMenu;


    private void OnEnable() {
        GameManager.OnPauseToggled += ToggleMainMenu;
        GameManager.OnPauseToggled += ToggleWaitButton;

    }

    private void OnDisable() {
        GameManager.OnPauseToggled -= ToggleMainMenu;
        GameManager.OnPauseToggled -= ToggleWaitButton;

    }

    public void ToggleWaitButton(bool active) {
        waitButton.gameObject.SetActive(!active);
    }

    public void ToggleMainMenu(bool active) {
        mainMenu.SetActive(active);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
