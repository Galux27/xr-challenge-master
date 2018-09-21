using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterEnemy : MonoBehaviour
{

    Vector3 startPosition = Vector3.zero;
    Rigidbody rb;
    float d = 0.0f;
    float speed = 4.0f;

    [SerializeField]
    List<Transform> missileSpawnPoints;

    [SerializeField]
    GameObject missilePrefab;
    float attackTimer = 1.0f;
    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        startPosition = this.transform.position;
    }

    private void Update()
    {
        d= Vector3.Distance(this.transform.position, PlayerMovement.me.transform.position);

        if (canWeAttackEnemy())
        {
            Debug.Log("Helicopter can attack player");
            moveToPlayer();
            attackPlayer();
        }
       
    }

    void moveToPlayer()
    {
        Vector3 dir = PlayerMovement.me.transform.position - this.transform.position;
        if(d>5)
        {
            rb.AddForce(dir.normalized * speed, ForceMode.Acceleration);
        }
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(dir*-1), speed * Time.deltaTime);
    }

    bool canWeAttackEnemy()
    {
        if (d < 10)
        {
            Vector3 dir = PlayerMovement.me.transform.position - this.transform.position;
            Debug.DrawRay(this.transform.position, dir.normalized * d, Color.red);
            RaycastHit hit;
            if(Physics.Raycast(this.transform.position,dir, out hit,Mathf.Infinity))
            {
                if (hit.collider.tag == "Player")
                {
                    return true;
                }
            }
        }
        return false;
    }

    void attackPlayer()
    {
        attackTimer -= Time.deltaTime;
        if(attackTimer<=0)
        {
            GameObject g = (GameObject) Instantiate(missilePrefab, missileSpawnPoints[Random.Range(0, missileSpawnPoints.Count)].transform.position, this.transform.rotation);
            g.GetComponent<Missile>().SetCreator(this.gameObject);
            Debug.Log("Created missile");
            attackTimer = 1.0f;
        }
    }
}
