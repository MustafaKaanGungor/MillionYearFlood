using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool isPaused = false;

    public delegate void OnPauseToggledDelegate(bool isPaused);
    public static event OnPauseToggledDelegate OnPauseToggled;

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
}
