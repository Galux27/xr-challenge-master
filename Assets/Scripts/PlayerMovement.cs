using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement me;

    [SerializeField]
    GameObject childVisual;
    private void Awake()
    {
        me = this;
    }

    [SerializeField]
    float playerSpeed = 5.0f;

    [SerializeField]
    float velocityLimit = 5.0f;

    Rigidbody rb;
    bool dead = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        movePlayer();
    }

    private void Update()
    {
        if(dead==true)
        {
            return;
        }
        transformDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (transformDir != Vector3.zero)
        {
            childVisual.transform.rotation = Quaternion.Slerp(childVisual.transform.rotation, Quaternion.LookRotation(transformDir * -1), playerSpeed * Time.deltaTime);
        }
    }
    Vector3 transformDir;
    void movePlayer()
    {
        if (rb.velocity.magnitude >= velocityLimit)
        {
            return;
        }

        rb.AddForce(transformDir*playerSpeed, ForceMode.Acceleration);

       
    }

    public void setDead()
    {
        dead = true;
    }
}
