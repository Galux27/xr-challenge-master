using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public static List<Missile> instances;


    [SerializeField]
    int damage = 50;

    Rigidbody rb;

    GameObject myCreator;

    private void Awake()
    {
        if(Missile.instances==null)
        {
            Missile.instances = new List<Missile>();
        }
        Missile.instances.Add(this);
    }

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
        if (collision.gameObject == myCreator || collision.gameObject.transform.root.gameObject==myCreator)
        {
            return;
        }

        Debug.Log("Missile collided with " + collision.collider.gameObject.name);
        if (collision.gameObject.tag == "Player")
        {
            //deal damage
            Instantiate(PrefabStore.me.explosionEffect, collision.contacts[0].point, Quaternion.Euler(0, 0, 0));
            PlayerHealth.me.dealDamage(damage);
        }else if (collision.gameObject.tag == "Missile")
        {

            Destroy(collision.gameObject);
        }else if (collision.gameObject.tag == "EnemyHeli")
        {
            Instantiate(PrefabStore.me.explosionEffect, collision.contacts[0].point, Quaternion.Euler(0, 0, 0));
            GameController.me.increaseScore(100);
            Destroy(collision.gameObject);
        }
        else if(collision.gameObject.tag== "SAM")
        {
            Instantiate(PrefabStore.me.explosionEffect, collision.contacts[0].point, Quaternion.Euler(0, 0, 0));
            GameController.me.increaseScore(50);

            Destroy(collision.gameObject);
        }
        Instantiate(PrefabStore.me.explosionEffect, this.transform.position, Quaternion.Euler(0, 0, 0));

        Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        ParticleSystem missileSmoke = this.GetComponentInChildren<ParticleSystem>();
        missileSmoke.gameObject.transform.parent = null;
        missileSmoke.loop= false;
        missileSmoke.Stop();
        Missile.instances.Remove(this);
    }
}
