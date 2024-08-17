using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{

    public enum ResourceType { Wood, Coal, Iron, Water }
    private Dictionary<ResourceType, int> resources = new Dictionary<ResourceType, int>();
    public UIManager uiManager;
    
    void Start()
    {

        foreach (ResourceType type in System.Enum.GetValues(typeof(ResourceType)))
        {
            resources[type] = 0;
        }

        AddResource(ResourceType.Coal, 20);
        AddResource(ResourceType.Iron, 20);
        AddResource(ResourceType.Wood, 20);
        AddResource(ResourceType.Water, 20);


    }
    private void Update() 
    {
        uiManager.UpdateResourceUI(resources);
    }


    public void AddResource(ResourceType type, int amount)
    {
        resources[type] += amount;
    }

    public bool RemoveResource(ResourceType type, int amount)
    {
        if (resources[type] >= amount)
        {
            resources[type] -= amount;
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
}
