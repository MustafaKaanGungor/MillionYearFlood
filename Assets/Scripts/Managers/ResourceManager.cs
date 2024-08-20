using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ResourceManager : MonoBehaviour
{
    public enum ResourceType { Wood, Coal, Iron, Water, Food, Humans}
    private Dictionary<ResourceType, int> resources = new Dictionary<ResourceType, int>();
    private Dictionary<ResourceType, int> resourcesCapacity = new Dictionary<ResourceType, int>();

    public UIManager uiManager;

    public int totalHumanCount = 186;

    [SerializeField] private GameManager gameManager;

    public delegate void OnResourceChangedDelegate(Dictionary<ResourceType, int> resources, Dictionary<ResourceType, int> resourceCapacity);
    public static event OnResourceChangedDelegate OnResourceChanged;

    private float Humantimer;
    private float foodConsumeTime = 0f;
    private float foodConsumeDur = 3f;
    private int foodConsumption = 1;

    private int startingHumanCount = 36;

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
        resourcesCapacity[ResourceType.Humans] = 300;

        AddResource(ResourceType.Coal, 40);
        AddResource(ResourceType.Iron, 10);
        AddResource(ResourceType.Wood, 10);
        AddResource(ResourceType.Water, 30);
        AddResource(ResourceType.Food, 20);
        AddResource(ResourceType.Humans, startingHumanCount);
    }

    private void Update() 
    {
        int food = GetResourceAmount(ResourceType.Food);
        int HumanDiff =  food - GetResourceAmount(ResourceType.Humans); 
        if(food == 0)
        {
            Humantimer += Time.deltaTime;
            if (Humantimer > 1)
            {
                int amount = 1; 
                Humantimer = 0;
                RemoveResource(ResourceType.Humans, amount);

                if (GetResourceAmount(ResourceType.Humans) <= 0) {
                    gameManager.GameOver("açlıktan öldün");
                }
            }
        }

        foodConsumeTime += Time.deltaTime;

        int humanCount = GetResourceAmount(ResourceManager.ResourceType.Humans);

        foodConsumeDur = 2 / ((humanCount + 1f) / startingHumanCount);

        if (foodConsumeTime >= foodConsumeDur) {
            foodConsumeTime = 0f;
            RemoveResource(ResourceType.Food, 1);
        }

    }

    public void AddResource(ResourceType type, int amount)
    {
        if(amount > resourcesCapacity[type] - resources[type]) {
            resources[type] = resourcesCapacity[type];
            OnResourceChanged?.Invoke(resources, resourcesCapacity);
        } else {
            resources[type] += amount;
            OnResourceChanged?.Invoke(resources, resourcesCapacity);
        }
    
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
            resources[type] = 0;
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
