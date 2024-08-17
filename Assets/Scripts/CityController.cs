using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityController : MonoBehaviour
{
    public Animator animator;

    public float speed = 1f;
    public float maxSpeed = 1f;
    private float acceleration = 1f;

    public bool isWaiting = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isWaiting) return;

        if(speed < maxSpeed) {
            Accelerate();
        }
        else if(speed > maxSpeed) {
            Decelerate();
        }

        MoveUp();

        animator.speed = speed;
    }

    public void ToggleWait() {
        isWaiting = !isWaiting;

        if (isWaiting) {
            speed = 0f;
            //acceleration = 1f;
            animator.speed = 0f;
        }
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
}
