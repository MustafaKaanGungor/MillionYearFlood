using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class CityController : MonoBehaviour
{
    [System.Serializable]
    public struct EngineTier {
        public int tier;
        public float maxSpeed;
        public float defMaxSpeed;
        public float engineEfficiencyMult; // higher value will consume more coal 
        public EngineTier(int tier, float maxSpeed, float engineEfficiencyMult) {
            this.tier = tier;
            this.maxSpeed = maxSpeed;
            this.engineEfficiencyMult = engineEfficiencyMult;
            this.defMaxSpeed = maxSpeed;
        }
    }

    public Animator animator;

    public GameObject silo;
    public GameObject greenHouse;
    public GameObject blacksmith;


    public EngineTier[] engines;
    public EngineTier curEngine; //[HideInInspector] 

    public float speed = 1f;
    //public float maxSpeed = 1f;

    //private float defMaxSpeed;
    private float acceleration = 0.5f;
    //private int coalConsumption = 1;
    private float coalConsumeDur = 1f;
    private float coalConsumeTime = 0f;

    private float foodProduceTime = 0f;


    private int waterConsumption = 1;
    private float waterConsumeDur = 2f;
    private float waterConsumeTime = 0f;
    public bool isWaiting = false;
    private bool canWait = true;
    public bool canMoveAndGather = false;

    [SerializeField] private ResourceManager resourceManager;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private UIManager uiManager;
    public ParticleSystem engineSmoke;

    [HideInInspector] public int woodPickAmount = 5;
    [HideInInspector] public int coalPickAmount = 5;
    [HideInInspector] public int ironPickAmount = 5;
    [HideInInspector] public int waterPickAmount = 5;
    [HideInInspector] public int foodPickAmount = 5;
    [HideInInspector] public int humanPickAmount = 25;

    [HideInInspector] public float resourcePickupDur = 2f;


    private int tier = 1;
    private float defSmokeEmitionRate;
    private float ironTimer;
    private float coalTimer;
    private float woodTimer;
    private float waterTimer;
    private float foodTimer;
    private float humanTimer;

    Rigidbody2D rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        //defMaxSpeed = curEngine.maxSpeed;
        rigidbody = GetComponent<Rigidbody2D>();
        speed = 0;
        curEngine = engines[1];

        defSmokeEmitionRate = engineSmoke.emission.rateOverTime.constant;
    }

    // Update is called once per frame
    void Update(){
        if (rigidbody.IsSleeping()) {
            rigidbody.WakeUp();
        }

        if (speed < curEngine.maxSpeed) {
            Accelerate();
        }
        else if(speed > curEngine.maxSpeed) {
            Decelerate();
        }

        animator.speed = speed;
        var emission = engineSmoke.emission;
        emission.rateOverTime = defSmokeEmitionRate * speed;

        if (greenHouse.activeSelf ) {
            foodProduceTime += Time.deltaTime;
            if(foodProduceTime >= 10f && resourceManager.GetResourceAmount(ResourceManager.ResourceType.Water) > 0) {
                foodProduceTime = 0;
                int water = resourceManager.GetResourceAmount(ResourceManager.ResourceType.Water);
                int amount = water > 5 ? 10 : water*2;
                resourceManager.AddResource(ResourceManager.ResourceType.Food, water);
                resourceManager.RemoveResource(ResourceManager.ResourceType.Water, 5);
            }
        }

        if (speed == 0) return;

        MoveUp();

        coalConsumeTime += Time.deltaTime;
        coalConsumeDur = 2 / speed / curEngine.engineEfficiencyMult;

        if(coalConsumeTime >= coalConsumeDur) {
            coalConsumeTime = 0f;
            //int amount = (int)(coalConsumption * speed * 10) / 10;
            UseCoal(1);
        }
    }

    public void ToggleWait() {
        if (!canWait) return;

        if (isWaiting && resourceManager.GetResourceAmount(ResourceManager.ResourceType.Coal) == 0) return;

        isWaiting = !isWaiting;

        curEngine.maxSpeed = isWaiting ? 0f : curEngine.defMaxSpeed;
    }

    public void MoveUp() {
        transform.position += Vector3.up * Time.deltaTime * speed;
    }

    public void Accelerate() {
        speed += Time.deltaTime * acceleration;

        speed = speed > curEngine.maxSpeed ? curEngine.maxSpeed : speed;
    }

    public void Decelerate() {
        speed -= Time.deltaTime * acceleration;

        speed = speed < curEngine.maxSpeed ? curEngine.maxSpeed : speed;

    }

    private void UseCoal(int amount) {
        resourceManager.RemoveResource(ResourceManager.ResourceType.Coal, amount);

        if (resourceManager.GetResourceAmount(ResourceManager.ResourceType.Coal) == 0 && !isWaiting) {
            ToggleWait();   
        }
    }

    private void UseWater(int waterAmount) {
        resourceManager.RemoveResource(ResourceManager.ResourceType.Water, waterAmount);
    }

    public void UnlockSecondTier() {
        animator.SetBool("isTier2", true);
        tier = 2;
        transform.localScale = Vector3.one * 1.2f;
    }
    public void EnableBuilding(GameObject obj) {
        obj.SetActive(true);
    }

    public void ChangeEngine(System.Single index) {
        float prevSpeed = curEngine.maxSpeed;

        curEngine = engines[(int)index];

        if(curEngine.tier == 1 && canMoveAndGather) {
            isWaiting = true;
            canWait = false;
        }
        else if(speed > 0) {
            isWaiting = false;
            canWait = true;
        }

        if (!isWaiting | canMoveAndGather) return;

        curEngine.maxSpeed = prevSpeed;
    }

    public void OverHeatEngines(float maxSpeed, float dur) {

        StartCoroutine(_OverHeatEngines(maxSpeed, dur));
    }

    private IEnumerator _OverHeatEngines(float maxSpeed, float dur) {
        canWait = false;

        float tempMaxSpeed = this.curEngine.maxSpeed;
        float defAcceleration = acceleration;

        this.curEngine.maxSpeed = maxSpeed;
        acceleration *= 3;

        yield return new WaitForSeconds(dur);

        this.curEngine.maxSpeed = tempMaxSpeed;

        acceleration = defAcceleration;
        canWait = true;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("End")) {
            curEngine.maxSpeed /= 6f;
        }

        if (collision.gameObject.CompareTag("Flood")) {
            // Game over
            gameManager.GameOver("selden dolayı öldün");
            return;
        }

    }

    private void OnTriggerStay2D(Collider2D collision) {

        if(collision.gameObject.CompareTag("Iron") && isWaiting) {
            ironTimer += Time.deltaTime;
            uiManager.ResourceGatheringBar(ResourceManager.ResourceType.Iron, ironTimer/ resourcePickupDur);
            if(ironTimer > resourcePickupDur) {
                ironTimer = 0;
                resourceManager.AddResource(ResourceManager.ResourceType.Iron, ironPickAmount);
                Debug.Log(resourceManager.GetResourceAmount(ResourceManager.ResourceType.Iron));
            }
        }

        if (collision.gameObject.CompareTag("Coal") && isWaiting) {
            coalTimer += Time.deltaTime;
            uiManager.ResourceGatheringBar(ResourceManager.ResourceType.Coal, coalTimer/ resourcePickupDur);
            if (coalTimer > resourcePickupDur) {
                coalTimer = 0;
                resourceManager.AddResource(ResourceManager.ResourceType.Coal, coalPickAmount);
            }
        }

        if (collision.gameObject.CompareTag("Wood") && isWaiting) {
            woodTimer += Time.deltaTime;
            uiManager.ResourceGatheringBar(ResourceManager.ResourceType.Wood, woodTimer/ resourcePickupDur);
            if (woodTimer > resourcePickupDur) {
                woodTimer = 0;
                resourceManager.AddResource(ResourceManager.ResourceType.Wood, woodPickAmount);
            }
        }
        if (collision.gameObject.CompareTag("Water") && isWaiting) {
            waterTimer += Time.deltaTime;
            uiManager.ResourceGatheringBar(ResourceManager.ResourceType.Water, waterTimer/ resourcePickupDur);
            if (waterTimer > resourcePickupDur) {
                waterTimer = 0;
                resourceManager.AddResource(ResourceManager.ResourceType.Water, waterPickAmount);
            }
        }
        if (collision.gameObject.CompareTag("Food") && isWaiting) {
            foodTimer += Time.deltaTime;
            uiManager.ResourceGatheringBar(ResourceManager.ResourceType.Food, foodTimer/ resourcePickupDur);
            if (foodTimer > resourcePickupDur) {
                foodTimer = 0;
                resourceManager.AddResource(ResourceManager.ResourceType.Food, foodPickAmount);
            }
        }
        if (collision.gameObject.CompareTag("Human") && isWaiting) {
            humanTimer += Time.deltaTime;
            uiManager.ResourceGatheringBar(ResourceManager.ResourceType.Humans, humanTimer / resourcePickupDur);
            if (humanTimer > 2) {
                humanTimer = 0;
                resourceManager.AddResource(ResourceManager.ResourceType.Humans, 25);

                Destroy(collision.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {

        if (collision.gameObject.CompareTag("Iron")) {
            uiManager.SetResourceGatheringBarActive(ResourceManager.ResourceType.Iron);
        }
        if(collision.gameObject.CompareTag("Wood")) {
            uiManager.SetResourceGatheringBarActive(ResourceManager.ResourceType.Wood);
        }
        if(collision.gameObject.CompareTag("Coal")) {
            uiManager.SetResourceGatheringBarActive(ResourceManager.ResourceType.Coal);
        }
        if(collision.gameObject.CompareTag("Water")) {
            uiManager.SetResourceGatheringBarActive(ResourceManager.ResourceType.Water);
        }
        if(collision.gameObject.CompareTag("Food")) {
            uiManager.SetResourceGatheringBarActive(ResourceManager.ResourceType.Food);
        }
        if(collision.gameObject.CompareTag("Human")) {
            uiManager.SetResourceGatheringBarActive(ResourceManager.ResourceType.Humans);
        }


        if (collision.gameObject.CompareTag("End")) {
            gameManager.Victory();
        }
    }
}
