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

    private int foodConsumption = 1;
    private float foodConsumeDur = 3f;
    private float foodConsumeTime = 0f;
    private float foodProduceTime = 0f;


    private int waterConsumption = 1;
    private float waterConsumeDur = 2f;
    private float waterConsumeTime = 0f;
    public bool isWaiting = false;

    [SerializeField] private ResourceManager resourceManager;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private UIManager uiManager;


    private int tier = 1;
    private float ironTimer;
    private float coalTimer;
    private float woodTimer;
    private float waterTimer;
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

        foodConsumeTime += Time.deltaTime;

        if (foodConsumeTime >= foodConsumeDur) {
            foodConsumeTime = 0f;
            int foodAmount = (int)(foodConsumption * 10) / 10;

            UseFood(foodAmount);
        }

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
        resourceManager.RemoveResource(ResourceManager.ResourceType.Food, foodAmount);
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
            uiManager.ResourceGatheringBar(ironTimer/2);
            if(ironTimer > 2) {
                ironTimer = 0;
                resourceManager.AddResource(ResourceManager.ResourceType.Iron, 5);
                Debug.Log(resourceManager.GetResourceAmount(ResourceManager.ResourceType.Iron));
            }
        }

        if (collision.gameObject.CompareTag("Coal") && isWaiting) {
            coalTimer += Time.deltaTime;
            uiManager.ResourceGatheringBar(coalTimer/2);
            if (coalTimer > 2) {
                coalTimer = 0;
                resourceManager.AddResource(ResourceManager.ResourceType.Coal, 5);
            }
        }

        if (collision.gameObject.CompareTag("Wood") && isWaiting) {
            woodTimer += Time.deltaTime;
            uiManager.ResourceGatheringBar(woodTimer/2);
            if (woodTimer > 2) {
                woodTimer = 0;
                resourceManager.AddResource(ResourceManager.ResourceType.Wood, 5);
            }
        }
        if (collision.gameObject.CompareTag("Water") && isWaiting) {
            waterTimer += Time.deltaTime;
            uiManager.ResourceGatheringBar(waterTimer/2);
            if (waterTimer > 2) {
                waterTimer = 0;
                resourceManager.AddResource(ResourceManager.ResourceType.Water, 5);
            }
        }

        if (collision.gameObject.CompareTag("Human") && isWaiting) {
            humanTimer += Time.deltaTime;
            uiManager.ResourceGatheringBar(humanTimer / 2);
            if (humanTimer > 2) {
                humanTimer = 0;
                resourceManager.AddResource(ResourceManager.ResourceType.Humans, 10);

                Destroy(collision.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {

        if (collision.gameObject.CompareTag("Iron") || collision.gameObject.CompareTag("Wood")
            || collision.gameObject.CompareTag("Coal") || collision.gameObject.CompareTag("Water")) {
            uiManager.SetResourceGatheringBarActive();
        }

        if (collision.gameObject.CompareTag("End")) {
            gameManager.Victory();
        }
    }
}
