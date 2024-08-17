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

    public bool isWaiting = false;


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

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Flood")) {
            // Game over
            GameManager.instance.GameOver();
        }
    }
}
