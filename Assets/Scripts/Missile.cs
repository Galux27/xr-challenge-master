using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField]
    int damage = 50;

    Rigidbody rb;

    GameObject myCreator;

    public void SetCreator(GameObject g)
    {
        myCreator = g;
    }

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(rb.velocity.magnitude < 5)
        {
            rb.AddForce((transform.forward).normalized * 7, ForceMode.Acceleration);
        }
    }

    private void Update()
    {
        Quaternion rot = Quaternion.LookRotation(PlayerMovement.me.transform.position - this.transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, 5 * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == myCreator)
        {
            return;
        }

        //Debug.Log("Missile collided with " + collision.collider.gameObject.name);
        if (collision.gameObject.tag == "Player")
        {
            //deal damage
            PlayerHealth.me.dealDamage(damage);
        }
        Destroy(this.gameObject);
    }
}
