using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    float playerSpeed = 5.0f;

    [SerializeField]
    float velocityLimit = 5.0f;

    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        movePlayer();
    }

    void movePlayer()
    {
        if (rb.velocity.magnitude >= velocityLimit)
        {
            return;
        }

        Vector3 transformDir = new Vector3(Input.GetAxis("Horizontal"),0, Input.GetAxis("Vertical"));
        rb.AddForce(transformDir*playerSpeed, ForceMode.Acceleration);
    }
}
