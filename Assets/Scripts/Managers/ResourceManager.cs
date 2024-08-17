using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{

    public enum ResourceType { Wood, Stone, Coal, Iron }
    private Dictionary<ResourceType, int> resources = new Dictionary<ResourceType, int>();
    
    void Start()
    {

        foreach (ResourceType type in System.Enum.GetValues(typeof(ResourceType)))
        {
            resources[type] = 0;
        }

        AddResource(ResourceType.Coal, 20);
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
