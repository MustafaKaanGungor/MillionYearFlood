using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Resource {

    public ResourceManager.ResourceType type;
    public int cost;

}

public class ResourceManager : MonoBehaviour
{
    public enum ResourceType { Wood, Coal, Iron, Water, Food, Humans}
    private Dictionary<ResourceType, int> resources = new Dictionary<ResourceType, int>();
    private Dictionary<ResourceType, int> resourcesCapacity = new Dictionary<ResourceType, int>();

    public UIManager uiManager;

    public int totalHumanCount = 115;

    public delegate void OnResourceChangedDelegate(Dictionary<ResourceType, int> resources);
    public static event OnResourceChangedDelegate OnResourceChanged;

    void Start()
    {
        foreach (ResourceType type in System.Enum.GetValues(typeof(ResourceType)))
        {
            resources[type] = 0;
        }

        resourcesCapacity[ResourceType.Coal] = 50;
        resourcesCapacity[ResourceType.Iron] = 50;
        resourcesCapacity[ResourceType.Wood] = 50;
        resourcesCapacity[ResourceType.Water] = 50;
        resourcesCapacity[ResourceType.Food] = 50;
        resourcesCapacity[ResourceType.Humans] = 50;


        AddResource(ResourceType.Coal, 40);
        AddResource(ResourceType.Iron, 20);
        AddResource(ResourceType.Wood, 20);
        AddResource(ResourceType.Water, 20);
        AddResource(ResourceType.Food, 20);
        AddResource(ResourceType.Humans, 5);
    }

    private void Update() 
    {
        uiManager.UpdateResourceUI(resources);
    }

    public void AddResource(ResourceType type, int amount)
    {
        resources[type] += amount;
        OnResourceChanged?.Invoke(resources);

    }

    public bool RemoveResource(ResourceType type, int amount){
        if (resources[type] >= amount)
        {
            resources[type] -= amount;

            OnResourceChanged?.Invoke(resources);

            return true;
        }
        else
        {
            return false;
        }
    }

    public int GetResourceAmount(ResourceType type)
    {
        return resources[type];
    }

    public void AddResourceCapacity(ResourceType type, int amount) {
        resources[type] += amount;
    }

    public int GetResourceCapacity(ResourceType type) {
        return resourcesCapacity[type];
    }
}
