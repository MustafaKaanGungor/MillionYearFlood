using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityController : MonoBehaviour
{
    public Animator animator;

    public float speed = 1f;
    public float maxSpeed = 1f;

    private float defMaxSpeed;
    private float acceleration = 0.5f;
    private int coalConsumption = 1;
    private float coalConsumeDur = 1f;
    private float coalConsumeTime = 0f;
    public bool isWaiting = false;

    [SerializeField] private ResourceManager resourceManager;
    [SerializeField] private GameManager gameManager;

    private int tier = 1;
    private float ironTimer;
    private float coalTimer;
    private float woodTimer;
    private float waterTimer;



    // Start is called before the first frame update
    void Start()
    {
        defMaxSpeed = maxSpeed;
    }

    // Update is called once per frame
    void Update(){
        if(speed < maxSpeed) {
            Accelerate();
        }
        else if(speed > maxSpeed) {
            Decelerate();
        }

        animator.speed = speed;

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
        Debug.Log("coal consumed: " + amount);
        resourceManager.RemoveResource(ResourceManager.ResourceType.Coal, amount);
        Debug.Log("coal remain: " + resourceManager.GetResourceAmount(ResourceManager.ResourceType.Coal));

        if (resourceManager.GetResourceAmount(ResourceManager.ResourceType.Coal) == 0 && !isWaiting) {
            ToggleWait();   
        }
    }

    public void UnlockSecondTier() {
        animator.SetBool("isTier2", true);
        tier = 2;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Flood")) {
            // Game over
            gameManager.GameOver();
        }
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if(collision.gameObject.CompareTag("Iron") && isWaiting) {
            ironTimer += Time.deltaTime;
            if(ironTimer > 2) {
                ironTimer = 0;
                resourceManager.AddResource(ResourceManager.ResourceType.Iron, 5);
                Debug.Log(resourceManager.GetResourceAmount(ResourceManager.ResourceType.Iron));
            }
        }

        if (collision.gameObject.CompareTag("Coal") && isWaiting) {
            coalTimer += Time.deltaTime;
            if (coalTimer > 2) {
                coalTimer = 0;
                resourceManager.AddResource(ResourceManager.ResourceType.Coal, 5);
                Debug.Log("total coal amount: " + resourceManager.GetResourceAmount(ResourceManager.ResourceType.Coal));
            }
        }
        if (collision.gameObject.CompareTag("Wood") && isWaiting) {
            woodTimer += Time.deltaTime;
            if (woodTimer > 2) {
                woodTimer = 0;
                resourceManager.AddResource(ResourceManager.ResourceType.Wood, 5);
                Debug.Log("total coal amount: " + resourceManager.GetResourceAmount(ResourceManager.ResourceType.Coal));
            }
        }
        if (collision.gameObject.CompareTag("Water") && isWaiting) {
            waterTimer += Time.deltaTime;
            if (waterTimer > 2) {
                waterTimer = 0;
                resourceManager.AddResource(ResourceManager.ResourceType.Water, 5);
                Debug.Log("total coal amount: " + resourceManager.GetResourceAmount(ResourceManager.ResourceType.Coal));
            }
        }
    }
}
