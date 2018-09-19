using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScoreController : MonoBehaviour
{
    public static ScoreController me;

    [SerializeField]
    Text scoreDisplay,remainingStars;
    int score = 0;
    int pickupsFound = 0;
    Pickup[] pickups;

    private void Awake()
    {
        me = this;
    }

    private void Start()
    {
        pickups = FindObjectsOfType<Pickup>();
        foreach (Pickup p in pickups) {
            p.OnPickUp += addScore;
        }

    }

    void addScore(Pickup p)
    {
        Debug.Log("Score being increased");
        score += p.ScoreValue;
        pickupsFound++;
    }
    // Update is called once per frame
    void Update()
    {
        scoreDisplay.text = "Score: " + score.ToString();
        remainingStars.text = "Start Remaining : " + (pickups.Length - pickupsFound).ToString() + "/" + pickups.Length.ToString();
    }

    public bool isGameFinished()
    {
        return pickupsFound == pickups.Length;
    }
}
