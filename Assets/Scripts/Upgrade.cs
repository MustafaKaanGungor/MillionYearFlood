using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    public Button getButton;
    public Image canYouGetImage;

    public ResourceCost[] cost;

    [SerializeField] private ResourceManager resourceManager;

    [SerializeField] private Color green;
    [SerializeField] private Color red;


    private void OnEnable() {
        ResourceManager.OnResourceChanged += UpdateStatus;
    }

    private void OnDisable() {
        ResourceManager.OnResourceChanged -= UpdateStatus;
    }

    private void UpdateStatus(Dictionary<ResourceManager.ResourceType, int> resources, Dictionary<ResourceManager.ResourceType, int> resourceCapacity) {
        foreach (var item in cost) {

            if (item.cost > resources[item.type]) {
                // Disable 

                canYouGetImage.color = red;
                getButton.interactable = false;
                return;
            }
        }

        // Enable
        canYouGetImage.color = green;
        getButton.interactable = true;
    }
}

[System.Serializable]
public class ResourceCost {
    public ResourceManager.ResourceType type;
    public int cost;
}
