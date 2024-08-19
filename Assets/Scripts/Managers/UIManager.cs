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
    public GameObject victoryPanel;

    public TextMeshProUGUI victoryText;
    public TextMeshProUGUI GameOverText;

    public TMP_Text[] ResourceTexts;
    public TMP_Text[] ResourceCapacityTexts;


    [SerializeField] private ResourceManager resourceManager;

    [SerializeField] private Button waitButton;

    [SerializeField] private Sprite leverUpImage;
    [SerializeField] private Sprite leverDownImage;

    [SerializeField] private Slider playerSlider;
    [SerializeField] private Slider waveSlider;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform waveTransform;
    [SerializeField] private Transform mapborder;

    [SerializeField] private Image[] resourceGatheringBar;



    private void OnEnable() {
        GameManager.OnPauseToggled += ToggleMainMenu;
        GameManager.OnPauseToggled += ToggleGameplayPanel;
        GameManager.OnGameOver += ToggleGameOverPanel;
        GameManager.OnVictory += ToggleVictoryPanel;

        ResourceManager.OnResourceChanged += UpdateResourceUI;

    }

    private void OnDisable() {
        GameManager.OnPauseToggled -= ToggleMainMenu;
        GameManager.OnPauseToggled -= ToggleGameplayPanel;
        GameManager.OnGameOver -= ToggleGameOverPanel;
        GameManager.OnVictory -= ToggleVictoryPanel;

        ResourceManager.OnResourceChanged -= UpdateResourceUI;
    }

    public void ToggleGameplayPanel(bool active) {
        gameplayPanel.SetActive(!active);
    }

    public void ToggleMainMenu(bool active) {
        mainMenu.SetActive(active);
    }

    public void ToggleGameOverPanel(string LoseText) {
        ToggleGameplayPanel(true);
        gameOverPanel.SetActive(true);
        GameOverText.text = LoseText;
    }

    public void ToggleVictoryPanel() {
        ToggleGameplayPanel(true);
        int humanCount = resourceManager.GetResourceAmount(ResourceManager.ResourceType.Humans);
        victoryText.text = string.Format(victoryText.text, humanCount, resourceManager.totalHumanCount - humanCount);
        victoryPanel.SetActive(true);
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void ReloadScene() 
    {
        SceneLoader.instance.LoadSceneWithIndex(1);
    }

    private void Update() {
        ProgressBars();
    } 
    public void ReturnToMainMenu() {
        SceneLoader.instance.LoadSceneWithIndex(0);
    }
    void ProgressBars() {
        float playerProgress = playerTransform.position.y / mapborder.position.y;
        playerSlider.value = playerProgress;
        float waveProgress = (waveTransform.position.y + 4) / mapborder.position.y;
        waveSlider.value = waveProgress;
    }

    public void ToggleWaitButtonSprite() {
        if(waitButton.image.sprite == leverUpImage) {
            waitButton.image.sprite = leverDownImage;
        }
        else if (waitButton.image.sprite == leverDownImage) {
            waitButton.image.sprite = leverUpImage;
        }
    }

    public void ResourceGatheringBar(ResourceManager.ResourceType resource,float percentage) {
        switch (resource)
        {
            case ResourceManager.ResourceType.Iron:
                if(resourceGatheringBar[0].IsActive()) {
                    resourceGatheringBar[0].fillAmount = percentage;
                } else {
                resourceGatheringBar[0].gameObject.SetActive(true);
                }
            break;
            case ResourceManager.ResourceType.Wood:
                if(resourceGatheringBar[1].IsActive()) {
                    resourceGatheringBar[1].fillAmount = percentage;
                } else {
                resourceGatheringBar[1].gameObject.SetActive(true);
                }
            break;
            case ResourceManager.ResourceType.Coal:
                if(resourceGatheringBar[2].IsActive()) {
                    resourceGatheringBar[2].fillAmount = percentage;
                } else {
                resourceGatheringBar[2].gameObject.SetActive(true);
                }
            break;
            case ResourceManager.ResourceType.Water:
                if(resourceGatheringBar[3].IsActive()) {
                    resourceGatheringBar[3].fillAmount = percentage;
                } else {
                resourceGatheringBar[3].gameObject.SetActive(true);
                }
            break;
            case ResourceManager.ResourceType.Food:
                if(resourceGatheringBar[4].IsActive()) {
                    resourceGatheringBar[4].fillAmount = percentage;
                } else {
                resourceGatheringBar[4].gameObject.SetActive(true);
                }
            break;
            case ResourceManager.ResourceType.Humans:
                if(resourceGatheringBar[5].IsActive()) {
                    resourceGatheringBar[5].fillAmount = percentage;
                } else {
                resourceGatheringBar[5].gameObject.SetActive(true);
                }
            break; 
        }

    }

    public void SetResourceGatheringBarActive(ResourceManager.ResourceType resource) {
        switch (resource)
        {
            case ResourceManager.ResourceType.Iron:
            ResourceGatheringBar(ResourceManager.ResourceType.Iron, 0f);
            resourceGatheringBar[0].gameObject.SetActive(false);
            break;
            case ResourceManager.ResourceType.Wood:
            ResourceGatheringBar(ResourceManager.ResourceType.Wood, 0f);
            resourceGatheringBar[0].gameObject.SetActive(false);
            break;
            case ResourceManager.ResourceType.Coal:
            ResourceGatheringBar(ResourceManager.ResourceType.Coal, 0f);
            resourceGatheringBar[0].gameObject.SetActive(false);
            break;
            case ResourceManager.ResourceType.Water:
            ResourceGatheringBar(ResourceManager.ResourceType.Water, 0f);
            resourceGatheringBar[0].gameObject.SetActive(false);
            break;
            case ResourceManager.ResourceType.Food:
            ResourceGatheringBar(ResourceManager.ResourceType.Food, 0f);
            resourceGatheringBar[0].gameObject.SetActive(false);
            break;
            case ResourceManager.ResourceType.Humans:
            ResourceGatheringBar(ResourceManager.ResourceType.Humans, 0f);
            resourceGatheringBar[0].gameObject.SetActive(false);
            break;
        }
    }

    

    public void UpdateResourceUI(Dictionary<ResourceManager.ResourceType, int> resources, Dictionary<ResourceManager.ResourceType, int> resourceCapacity)
    {
        foreach (ResourceManager.ResourceType type in System.Enum.GetValues(typeof(ResourceManager.ResourceType)))
    {
        int index = (int)type;
        if (index >= 0 && index < ResourceTexts.Length)
        {
            if (resources.ContainsKey(type))
            {
                ResourceTexts[index].text = resources[type].ToString();
                ResourceCapacityTexts[index].text = resourceCapacity[type].ToString();
                
                int currentResource = resources[type];
                //int maxCapacity = resourceCapacity[type];
                float ratio = (float)currentResource / 20;
                Color color = Color.Lerp(Color.red, Color.white, ratio);
                ResourceTexts[index].color = color;
            }
            else
            {
                ResourceTexts[index].text = "0";
                ResourceCapacityTexts[index].text = "0";
                ResourceTexts[index].color = Color.red;
            }
        }
    }
    }
}
