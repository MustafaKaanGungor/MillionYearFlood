using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityController : MonoBehaviour
{
    public Animator animator;

    public float speed = 1f;
    public bool isWaiting = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isWaiting) return;


        MoveUp();
    }

    public void ToggleWait() {
        isWaiting = !isWaiting;

        animator.speed = isWaiting ? 0f : 1f;
    }


    public void MoveUp() {
        transform.position += Vector3.up * Time.deltaTime * speed;
    }


}
