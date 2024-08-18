using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CityController : MonoBehaviour
{
    public Animator animator;

    public GameObject silo;
    public GameObject greenHouse;


    public float speed = 1f;
    public float maxSpeed = 1f;

    private float defMaxSpeed;
    private float acceleration = 0.5f;
    private int coalConsumption = 1;
    private float coalConsumeDur = 1f;
    private float coalConsumeTime = 0f;

    private float foodProduceTime = 0f;


    private int waterConsumption = 1;
    private float waterConsumeDur = 2f;
    private float waterConsumeTime = 0f;
    public bool isWaiting = false;
    private bool canWait = true;

    [SerializeField] private ResourceManager resourceManager;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private UIManager uiManager;


    private int tier = 1;
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
        defMaxSpeed = maxSpeed;
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update(){
        if (rigidbody.IsSleeping()) {
            rigidbody.WakeUp();
        }

        if (speed < maxSpeed) {
            Accelerate();
        }
        else if(speed > maxSpeed) {
            Decelerate();
        }

        animator.speed = speed;



        waterConsumeTime += Time.deltaTime;

        if (waterConsumeTime >= waterConsumeDur) {
            waterConsumeTime = 0f;
            int waterAmount = (int)(waterConsumption * 10) / 10;

            UseWater(waterAmount);
        }

        if (greenHouse.activeSelf) {
            foodProduceTime += Time.deltaTime;
            if(foodProduceTime >= 5f) {
                foodProduceTime = 0;
                resourceManager.AddResource(ResourceManager.ResourceType.Food, 10);
            }
        }

        if (speed == 0) return;

        MoveUp();

        coalConsumeTime += Time.deltaTime;

        if(coalConsumeTime >= coalConsumeDur) {
            coalConsumeTime = 0f;
            int amount = (int)(coalConsumption * speed * 10) / 10;

            UseCoal(amount);
        }
    }

    public void ToggleWait() {
        if (!canWait) return;

        isWaiting = !isWaiting;

        maxSpeed = isWaiting ? 0f : defMaxSpeed;
    }

    public void MoveUp() {
        transform.position += Vector3.up * Time.deltaTime * speed;
    }

    public void Accelerate() {
        speed += Time.deltaTime * acceleration;

        speed = speed > maxSpeed ? maxSpeed : speed;
    }

    public void Decelerate() {
        speed -= Time.deltaTime * acceleration;

        speed = speed < maxSpeed ? maxSpeed : speed;

    }

    private void UseCoal(int amount) {
        resourceManager.RemoveResource(ResourceManager.ResourceType.Coal, amount);

        if (resourceManager.GetResourceAmount(ResourceManager.ResourceType.Coal) == 0 && !isWaiting) {
            ToggleWait();   
        }
    }

    private void UseFood(int foodAmount) {
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

    public void OverHeatEngines(float maxSpeed, float dur) {

        StartCoroutine(_OverHeatEngines(maxSpeed, dur));
    }

    private IEnumerator _OverHeatEngines(float maxSpeed, float dur) {
        canWait = false;

        float tempMaxSpeed = this.maxSpeed;
        float defAcceleration = acceleration;

        this.maxSpeed = maxSpeed;
        acceleration *= 3;

        yield return new WaitForSeconds(dur);

        this.maxSpeed = tempMaxSpeed;

        acceleration = defAcceleration;
        canWait = true;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("End")) {
            maxSpeed /= 4f;
        }

        if (collision.gameObject.CompareTag("Flood")) {
            // Game over
            gameManager.GameOver();
            return;
        }


    }

    private void OnTriggerStay2D(Collider2D collision) {

        if(collision.gameObject.CompareTag("Iron") && isWaiting) {
            ironTimer += Time.deltaTime;
            uiManager.ResourceGatheringBar(ResourceManager.ResourceType.Iron, ironTimer/2);
            if(ironTimer > 2) {
                ironTimer = 0;
                resourceManager.AddResource(ResourceManager.ResourceType.Iron, 5);
                Debug.Log(resourceManager.GetResourceAmount(ResourceManager.ResourceType.Iron));
            }
        }

        if (collision.gameObject.CompareTag("Coal") && isWaiting) {
            coalTimer += Time.deltaTime;
            uiManager.ResourceGatheringBar(ResourceManager.ResourceType.Coal, coalTimer/2);
            if (coalTimer > 2) {
                coalTimer = 0;
                resourceManager.AddResource(ResourceManager.ResourceType.Coal, 5);
            }
        }

        if (collision.gameObject.CompareTag("Wood") && isWaiting) {
            woodTimer += Time.deltaTime;
            uiManager.ResourceGatheringBar(ResourceManager.ResourceType.Wood, woodTimer/2);
            if (woodTimer > 2) {
                woodTimer = 0;
                resourceManager.AddResource(ResourceManager.ResourceType.Wood, 5);
            }
        }
        if (collision.gameObject.CompareTag("Water") && isWaiting) {
            waterTimer += Time.deltaTime;
            uiManager.ResourceGatheringBar(ResourceManager.ResourceType.Water, waterTimer/2);
            if (waterTimer > 2) {
                waterTimer = 0;
                resourceManager.AddResource(ResourceManager.ResourceType.Water, 5);
            }
        }
        if (collision.gameObject.CompareTag("Food") && isWaiting) {
            foodTimer += Time.deltaTime;
            uiManager.ResourceGatheringBar(ResourceManager.ResourceType.Food, foodTimer/2);
            if (foodTimer > 2) {
                foodTimer = 0;
                resourceManager.AddResource(ResourceManager.ResourceType.Food, 5);
            }
        }
        if (collision.gameObject.CompareTag("Human") && isWaiting) {
            humanTimer += Time.deltaTime;
            uiManager.ResourceGatheringBar(ResourceManager.ResourceType.Humans, humanTimer / 2);
            if (humanTimer > 2) {
                humanTimer = 0;
                resourceManager.AddResource(ResourceManager.ResourceType.Humans, 10);

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
