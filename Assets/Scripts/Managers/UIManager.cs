using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    public GameObject gameplayPanel;
    public GameObject mainMenu;
    public GameObject gameOverPanel;

    public TMP_Text[] ResourceTexts;

    [SerializeField] private Slider playerSlider;
    [SerializeField] private Slider waveSlider;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform waveTransform;
    [SerializeField] private float mapborder;

    [SerializeField] private Image resourceGatheringBar;


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

    public void ReloadScene() 
    {
        SceneManager.LoadScene(0);
    }

    private void Update() {
        ProgressBars();
    } 

    void ProgressBars() {
        float playerProgress = playerTransform.position.y / mapborder;
        playerSlider.value = playerProgress;
        float waveProgress = (waveTransform.position.y + 4) / mapborder;
        waveSlider.value = waveProgress;
    }

    public void ResourceGatheringBar(float percentage) {
        if(resourceGatheringBar.IsActive()) {
            resourceGatheringBar.fillAmount = percentage;
        } else {
            resourceGatheringBar.gameObject.SetActive(true);
        }
    }

    public void SetResourceGatheringBarActive() {
        ResourceGatheringBar(0f);
        resourceGatheringBar.gameObject.SetActive(false);
    }

    

    public void UpdateResourceUI(Dictionary<ResourceManager.ResourceType, int> resources)
    {
        foreach (ResourceManager.ResourceType type in System.Enum.GetValues(typeof(ResourceManager.ResourceType)))
    {
        int index = (int)type;
        if (index >= 0 && index < ResourceTexts.Length)
        {
            if (resources.ContainsKey(type))
            {
                ResourceTexts[index].text = resources[type].ToString();
            }
            else
            {
                ResourceTexts[index].text = "0";
            }
        }
    }
    }
}
