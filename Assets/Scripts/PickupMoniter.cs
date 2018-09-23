using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupMoniter : MonoBehaviour
{
    Pickup myPickup;
    float shrinkTimer = 0.05f;
    float scale = 1.0f;
    private void Awake()
    {
        myPickup = this.GetComponent<Pickup>();
        
    }

    private void Update()
    {
        if (myPickup.IsCollected)
        {
            shrinkTimer -= Time.deltaTime;
            if (shrinkTimer <= 0)
            {
                scale -= 0.01f;
                this.transform.localScale = new Vector3(scale, scale, scale);
                if (scale <= 0)
                {
                    Destroy(this.gameObject);
                }
            }
        }
    }


}
