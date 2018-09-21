using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffectCleanup : MonoBehaviour
{
    ParticleSystem effect;
    // Start is called before the first frame update
    void Start()
    {
        effect = this.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (effect.isPlaying == false)
        {
            Destroy(this.gameObject);
        }
    }
}
