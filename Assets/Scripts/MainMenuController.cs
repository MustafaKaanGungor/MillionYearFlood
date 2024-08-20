using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.PlaySound(AudioManager.instance.music);
    }

    public void Play( ) {
        SceneLoader.instance.LoadSceneWithIndex(1);
    }
}
