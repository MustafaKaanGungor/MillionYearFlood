using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloodController : MonoBehaviour
{
    public GameManager gameManager;
    public float speed = 1f;

    private Camera cam;
    private AudioManager audioManager;
    private float distance;
    private float refDistance = 7.63f;

    private void Start() {
        cam = Camera.main;
        audioManager = AudioManager.instance;
        audioManager.PlaySound(audioManager.floodSound);

    }

    void Update()
    {

        MoveUp();

        distance = cam.transform.position.y - transform.position.y;
        float volume = (refDistance / distance / 3);
        audioManager.floodSound.source.volume = volume >= 0 ? volume : 0;
    }

    public void MoveUp() {
        transform.position += Vector3.up * Time.deltaTime * speed;
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Peak") && speed > 0) {

            speed -= (float)(Time.deltaTime * 0.07f);
        }

        if (collision.gameObject.CompareTag("City")) {
            gameManager.GameOver("Your people drowned in the flood.");
        }
    }



}
