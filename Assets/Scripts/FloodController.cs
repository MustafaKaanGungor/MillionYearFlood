using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloodController : MonoBehaviour
{
    public GameManager gameManager;
    public float speed = 1f;

    
    void Update()
    {
        MoveUp();
    }

    public void MoveUp() {
        transform.position += Vector3.up * Time.deltaTime * speed;
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("End") && speed > 0) {

            speed -= (float)(Time.deltaTime * 0.1);
        }

        if (collision.gameObject.CompareTag("City")) {
            gameManager.GameOver("Drowned in the flood.");
        }
    }



}
