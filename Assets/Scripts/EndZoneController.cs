using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndZoneController : MonoBehaviour
{
    [SerializeField]
    Material mat1, mat2;
    Renderer r;

    public static EndZoneController me;

    private void Awake()
    {
        me = this;
        r = this.gameObject.GetComponent<Renderer>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (GameController.me.isGameFinished())
            {
                Debug.Log("Game is done");
                r.material = mat1;
                GameController.me.setPlayerExited();
            }
            else
            {
                Debug.Log("Game is not done");
                r.material = mat2;
                GameController.me.enableObjectiveReminder();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        r.material = mat1;
    }

}
