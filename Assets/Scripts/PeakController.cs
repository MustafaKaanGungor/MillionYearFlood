
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeakController : MonoBehaviour
{
    public CityController cityController;

    private float speedBeforeEnter;

    private void OnTriggerEnter2D(Collider2D collision) {
        speedBeforeEnter = cityController.curEngine.defMaxSpeed;
        if (collision.gameObject.CompareTag("City")) {
            cityController.curEngine.maxSpeed /= 4f;
        }

        /*if (collision.gameObject.CompareTag("Flood")) {
            // Game over
            gameManager.GameOver("selden dolayı öldün");
            return;
        }*/
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("City")) {

            //if(cityController.isEnginesOverheated)

            cityController.curEngine.maxSpeed *= 2f;
        }

        /*if (collision.gameObject.CompareTag("Flood")) {
            // Game over
            gameManager.GameOver("selden dolayı öldün");
            return;
        }*/
    }
}
