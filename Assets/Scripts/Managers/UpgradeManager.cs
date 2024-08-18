using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UpgradeManager : MonoBehaviour
{
    public GameObject[] secondTierUpgrades;

    public Slider engineSlider;

    [SerializeField] private ResourceManager resourceManager;
    [SerializeField] private CityController cityController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Upgrade0() {

        resourceManager.RemoveResource(ResourceManager.ResourceType.Wood, 10);
        resourceManager.RemoveResource(ResourceManager.ResourceType.Iron, 10);
        resourceManager.RemoveResource(ResourceManager.ResourceType.Coal, 10);

        cityController.UnlockSecondTier();

        foreach (var item in secondTierUpgrades) {
            item.SetActive(true);
        }
    }


    public void UnlockSilo() {
        resourceManager.RemoveResource(ResourceManager.ResourceType.Wood, 10);
        resourceManager.RemoveResource(ResourceManager.ResourceType.Iron, 5);

        resourceManager.AddResourceCapacity(ResourceManager.ResourceType.Water, 50);
        resourceManager.AddResourceCapacity(ResourceManager.ResourceType.Food, 50);

        cityController.EnableBuilding(cityController.silo);

    }

    public void UnlockGreenHouse() {
        resourceManager.RemoveResource(ResourceManager.ResourceType.Wood, 10);
        resourceManager.RemoveResource(ResourceManager.ResourceType.Water, 50);

        cityController.EnableBuilding(cityController.greenHouse);

    }

    public void OverheatEngines() {
        resourceManager.RemoveResource(ResourceManager.ResourceType.Coal, 30);
        if (cityController.isWaiting)
            cityController.ToggleWait();

        cityController.OverHeatEngines(cityController.curEngine.maxSpeed * 3, 3f);
    }

    public void EngineUpgrade1() {
        resourceManager.RemoveResource(ResourceManager.ResourceType.Iron, 30);
        resourceManager.RemoveResource(ResourceManager.ResourceType.Wood, 10);

        engineSlider.maxValue = 2;
        cityController.ChangeEngine(2);
    }

    public void EngineUpgrade2() {
        resourceManager.RemoveResource(ResourceManager.ResourceType.Iron, 60);
        resourceManager.RemoveResource(ResourceManager.ResourceType.Wood, 20);
        engineSlider.maxValue = 3;

        cityController.ChangeEngine(3);
    }

}
