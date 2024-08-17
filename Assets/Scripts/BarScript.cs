using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarScript : MonoBehaviour
{
    [SerializeField] private Slider playerSlider;
    [SerializeField] private Slider waveSlider;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform waveTransform;
    [SerializeField] private float mapborder;
 
    void Update()
    {
        float playerProgress = playerTransform.position.y / mapborder;
        playerSlider.value = playerProgress;
        float waveProgress = (waveTransform.position.y + 17) / mapborder;
        waveSlider.value = waveProgress;
    }
}
