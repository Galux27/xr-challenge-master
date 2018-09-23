using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PlayerHealth : MonoBehaviour
{

    public static PlayerHealth me;

    [SerializeField]
    public Image healthDisp;

    [SerializeField]
    Text deathText;
    [SerializeField]
    GameObject deathDisplayParent;
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
            deathDisplayParent.SetActive(true);
            deathText.text = "You finished " + getNumberOfObjectivesComplete().ToString() + " objectives before dying. Your final score was " + GameController.me.getScore().ToString() + ".";
            Instantiate(PrefabStore.me.explosionEffect, PlayerMovement.me.transform.position, PlayerMovement.me.transform.rotation);
            PlayerMovement.me.setDead();
            //create explosion, reset stuff, fail
        }
    }

    int getNumberOfObjectivesComplete()
    {
        int i = 0;
        int baseVal = 10;
        for(int x = 1; x < PlayerPrefs.GetInt("Level"); x++)
        {
            i += baseVal;
            baseVal += 2;
        }

        i += GameController.me.getPickupsFound();
        return i;
    }

    public void deathRestart()
    {
        PlayerPrefs.SetInt("Score", 0);
        PlayerPrefs.SetInt("Level", 0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
