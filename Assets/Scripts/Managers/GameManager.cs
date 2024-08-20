using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{


    public bool isPaused = false;

    public delegate void OnPauseToggledDelegate(bool isPaused);
    public static event OnPauseToggledDelegate OnPauseToggled;

    public delegate void OnGameOverDelegate(string message);
    public static event OnGameOverDelegate OnGameOver;

    public delegate void OnVictoryDelegate();
    public static event OnVictoryDelegate OnVictory;

    public delegate void OnTutorialDelegate();
    public static event OnVictoryDelegate OnTutorial;





    void Awake() {
        
        isPaused = false;
    }


    void Start()
    {
        OnTutorial?.Invoke();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            TogglePause();
        }
    }

    public void TogglePause() {
        isPaused = !isPaused;

        Time.timeScale = isPaused ? 0 : 1f;

        if (isPaused) {
            AudioManager.instance.StopAll();
        }
        else {
            AudioManager.instance.ContinueAll();
        }


        OnPauseToggled?.Invoke(isPaused);
    }

    public void GameOver(string message) {

        OnGameOver?.Invoke(message);
        Time.timeScale = 0;

    }

    public void Victory() {
        OnVictory?.Invoke();
    }

}
