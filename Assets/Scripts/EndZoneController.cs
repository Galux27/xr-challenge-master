using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndZoneController : MonoBehaviour
{
    [SerializeField]
    Material mat1, mat2;
    Renderer r;

    private void Awake()
    {
        r = this.gameObject.GetComponent<Renderer>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (ScoreController.me.isGameFinished())
            {
                Debug.Log("Game is done");
                r.material = mat1;
            }
            else
            {
                Debug.Log("Game is not done");
                r.material = mat2;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        r.material = mat1;
    }

}
