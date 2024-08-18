using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ResourceManager : MonoBehaviour
{
    public enum ResourceType { Wood, Coal, Iron, Water, Food, Humans}
    private Dictionary<ResourceType, int> resources = new Dictionary<ResourceType, int>();
    private Dictionary<ResourceType, int> resourcesCapacity = new Dictionary<ResourceType, int>();

    public UIManager uiManager;

    public int totalHumanCount = 115;

    [SerializeField] private GameManager gameManager;

    public delegate void OnResourceChangedDelegate(Dictionary<ResourceType, int> resources, Dictionary<ResourceType, int> resourceCapacity);
    public static event OnResourceChangedDelegate OnResourceChanged;

    private float Humantimer;

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


        AddResource(ResourceType.Coal, 80);
        AddResource(ResourceType.Iron, 20);
        AddResource(ResourceType.Wood, 20);
        AddResource(ResourceType.Water, 20);
        AddResource(ResourceType.Food, 5);
        AddResource(ResourceType.Humans, 4);
    }

    private void Update() 
    {
        int food = GetResourceAmount(ResourceType.Food);
        int HumanDiff =  food - GetResourceAmount(ResourceType.Humans); 
        if(HumanDiff / 5 < 0 || food ==0)
        {
            Humantimer += Time.deltaTime;
            if (Humantimer > 5)
            {
                int amount = -HumanDiff / 5;

                if (-HumanDiff < 5 && food == 0)
                    amount = GetResourceAmount(ResourceType.Humans);

                RemoveResource(ResourceType.Humans, amount);
                Humantimer=0;

                if (GetResourceAmount(ResourceManager.ResourceType.Humans) <= 0) {
                    gameManager.GameOver();
                }
            }
        }
    }

    public void AddResource(ResourceType type, int amount)
    {
        resources[type] += amount;
        OnResourceChanged?.Invoke(resources, resourcesCapacity);

    }

    public bool RemoveResource(ResourceType type, int amount){
        if (resources[type] >= amount)
        {
            resources[type] -= amount;

            OnResourceChanged?.Invoke(resources, resourcesCapacity);

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
        resourcesCapacity[type] += amount;
        OnResourceChanged?.Invoke(resources, resourcesCapacity);
    }

    public int GetResourceCapacity(ResourceType type) {
        return resourcesCapacity[type];
    }
}
