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

    private void ApplyCost(Upgrade upgrade) {
        foreach (var item in upgrade.cost) {
            resourceManager.RemoveResource(item.type, item.cost);
        }
    }

    public void Upgrade0(Upgrade upgrade) {
        ApplyCost(upgrade);
        cityController.UnlockSecondTier();

        foreach (var item in secondTierUpgrades) {
            item.SetActive(true);
        }
    }


    public void UnlockSilo(Upgrade upgrade) {
        ApplyCost(upgrade);
        resourceManager.AddResourceCapacity(ResourceManager.ResourceType.Water, 50);
        resourceManager.AddResourceCapacity(ResourceManager.ResourceType.Food, 50);

        cityController.EnableBuilding(cityController.silo);

    }

    public void UnlockGreenHouse(Upgrade upgrade) {
        ApplyCost(upgrade);
        cityController.EnableBuilding(cityController.greenHouse);

    }

    public void OverheatEngines(Upgrade upgrade) {
        ApplyCost(upgrade);
        if (cityController.isWaiting)
            cityController.ToggleWait();

        cityController.OverHeatEngines(cityController.curEngine.maxSpeed * 3, 3f);
    }

    public void EngineUpgrade1(Upgrade upgrade) {
        ApplyCost(upgrade);
        engineSlider.maxValue = 2;
        //cityController.ChangeEngine(2);
    }

    public void EngineUpgrade2(Upgrade upgrade) {
        ApplyCost(upgrade);
        engineSlider.maxValue = 3;

        //cityController.ChangeEngine(3);
    }

}
