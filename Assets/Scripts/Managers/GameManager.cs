using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool isPaused = false;

    public delegate void OnPauseToggledDelegate(bool isPaused);
    public static event OnPauseToggledDelegate OnPauseToggled;

    public delegate void OnGameOverDelegate(string message);
    public static event OnGameOverDelegate OnGameOver;

    public delegate void OnVictoryDelegate();
    public static event OnVictoryDelegate OnVictory;


    void Awake() {
        Time.timeScale = 1f;
        isPaused = false;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            TogglePause();
        }
    }

    public void TogglePause() {
        isPaused = !isPaused;

        Time.timeScale = isPaused ? 0 : 1f;


        // Invoke on pause toggled event
        OnPauseToggled?.Invoke(isPaused);
    }

    public void GameOver(string message) {

        OnGameOver?.Invoke(message);
        Time.timeScale = 0;

    }

    public void Victory() {

        OnVictory?.Invoke();
        //Time.timeScale = 0;

    }
}
