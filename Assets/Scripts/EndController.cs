using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EndController : MonoBehaviour
{
    public GameManager gameManager;
    public CityController cityController;
    // Start is called before the first frame update
    void Start()
    {
        
    }



    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("City")) {
            collision.gameObject.transform.parent.DOScale(0.70f, 10f).SetEase(Ease.InSine); //.SetEase(Ease.InOutBack)
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        //speedBeforeEnter = cityController.curEngine.defMaxSpeed;
        if (collision.gameObject.CompareTag("City")) {
            Debug.Log("VICTORY");
            gameManager.Victory();
            cityController.ToggleWait();
            collision.gameObject.transform.parent.DOKill();
        }

        /*if (collision.gameObject.CompareTag("Flood")) {
            // Game over
            gameManager.GameOver("selden dolayı öldün");
            return;
        }*/
    }
}
