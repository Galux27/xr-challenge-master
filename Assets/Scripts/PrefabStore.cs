using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabStore : MonoBehaviour
{
    public static PrefabStore me;
    public GameObject hind, sam, missile, samMissile, explosionEffect,player;
    private void Awake()
    {
        me = this;
    }
}
