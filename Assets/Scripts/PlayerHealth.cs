using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHealth : MonoBehaviour
{

    public static PlayerHealth me;

    [SerializeField]
    public Image healthDisp;
    [SerializeField]
    int health = 1000;
    float startingWidth=0.0f;
    RectTransform r;
    private void Awake()
    {
        me = this;
        startingWidth = healthDisp.rectTransform.rect.width;
        r = this.GetComponent<RectTransform>();
    }

   

    public void dealDamage(int dam)
    {
        health -= dam;
        healthDisp.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (startingWidth /1000.0f) * (float)health);
        r.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (startingWidth / 1000.0f) * (float)health);
        if (health<=0)
        {
            //create explosion, reset stuff, fail
        }
    }
}
