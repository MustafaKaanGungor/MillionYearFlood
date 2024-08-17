using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    //public Upgrade[] allUpgrades;

    public GameObject[] secondTierUpgrades;

    [SerializeField] private ResourceManager resourceManager;
    [SerializeField] private CityController cityController;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    public void UpdateLockedUpgrades() {
       /* foreach (var item in allUpgrades) {

        }*/
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

        //cityController.EnableBuilding(cityController.silo);

    }

}
