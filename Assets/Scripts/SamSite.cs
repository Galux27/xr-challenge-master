using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamSite : MonoBehaviour
{
    public static List<SamSite> instances;

    [SerializeField]
    Transform missileSpawnPoint;

    float missileResetTimer = 0.0f;

    float d = 0.0f;

    private void Awake()
    {
        if (SamSite.instances == null)
        {
            SamSite.instances = new List<SamSite>();
        }
        SamSite.instances.Add(this);
    }

    private void Update()
    {
        d = Vector3.Distance(this.transform.position, PlayerMovement.me.transform.position);

        if (canWeAttackEnemy())
        {
            attackCountdown();
            Vector3 dir = PlayerMovement.me.transform.position - this.transform.position;
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(dir * -1),5.0f * Time.deltaTime);

        }
    }

    bool canWeAttackEnemy()
    {
        if (d < 10)
        {
            Vector3 dir = PlayerMovement.me.transform.position - this.transform.position;
            Debug.DrawRay(this.transform.position, dir.normalized * d, Color.red);
            RaycastHit hit;
            if (Physics.Raycast(this.transform.position, dir, out hit, Mathf.Infinity))
            {
                if (hit.collider.tag == "Player")
                {
                    return true;
                }
            }
        }
        return false;
    }

    void attackCountdown()
    {
        missileResetTimer -= Time.deltaTime;
        if (missileResetTimer <= 0)
        {
            StartCoroutine("createMissiles");
            missileResetTimer = 7.0f;
        }
    }

    IEnumerator createMissiles()
    {
        for(int x=0;x<3;x++)
        {
            GameObject missile = (GameObject)Instantiate(PrefabStore.me.samMissile, missileSpawnPoint.transform.position, this.transform.rotation);
            missile.GetComponent<Missile>().SetCreator(this.gameObject);
            yield return new WaitForSeconds(1.0f);
        }
    }

    private void OnDestroy()
    {
        SamSite.instances.Remove(this);
    }
}
