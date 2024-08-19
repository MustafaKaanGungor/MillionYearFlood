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

    public void BuildBlacksmithA(Upgrade upgrade) {
        ApplyCost(upgrade);

        cityController.EnableBuilding(cityController.blacksmith);

        cityController.woodPickAmount = 8;
        cityController.ironPickAmount = 8;
        cityController.coalPickAmount = 8;
        cityController.foodPickAmount = 8;
        cityController.waterPickAmount = 8;

        cityController.resourcePickupDur *= 1.2f; 
    }

    public void BuildBlacksmithB(Upgrade upgrade) {
        ApplyCost(upgrade);

        //cityController.EnableBuilding(cityController.blacksmith);

        cityController.woodPickAmount = 4;
        cityController.ironPickAmount = 4;
        cityController.coalPickAmount = 4;
        cityController.foodPickAmount = 4;
        cityController.waterPickAmount = 4;

        cityController.resourcePickupDur *= 0.6f;
    }

    public void GatherWhileInPower0(Upgrade upgrade) {
        ApplyCost(upgrade);

        cityController.canMoveAndGather = true;
    }

    public void BuildWatchTower(Upgrade upgrade) {
        ApplyCost(upgrade);

        cityController.IncreaseResourceGatherArea();
        cityController.EnableBuilding(cityController.watchTower);
    }
}
