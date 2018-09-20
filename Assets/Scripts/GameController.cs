﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameController : MonoBehaviour
{
    public static GameController me;

    [SerializeField]
    GameObject FinishedUI;

    [SerializeField]
    Text scoreDisplay,remainingStars,finishedText,timeTakenText,objectiveReminder;
    int score = 0;
    int pickupsFound = 0;
    Pickup[] pickups;
    bool gameFinished = false;//bool keeps track of whether the player has all the stars and has reached the end
    float timeTaken = 0.0f, objectiveRemindFade = 2.0f;

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
        remainingStars.text = "Stars Remaining : " + (pickups.Length - pickupsFound).ToString() + "/" + pickups.Length.ToString();
        timeTakenText.text = "Time : " + ((int)timeTaken).ToString();
        if (gameFinished==false)
        {
            timeTaken += Time.deltaTime;
        }

        if (objectiveReminder.enabled == true)
        {
            objectiveRemindFade -= Time.deltaTime;
            if(objectiveRemindFade<=0)
            {
                objectiveRemindFade = 2.0f;
                objectiveReminder.enabled = false;
            }
        }
    }

    public void loadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public bool isGameFinished()
    {
        return pickupsFound == pickups.Length;
    }

    public void setPlayerExited()
    {
        if (FinishedUI.activeInHierarchy == false)
        {
            FinishedUI.SetActive(true);
            finishedText.text = "It took you " + timeTaken.ToString() + " to find " + pickupsFound.ToString();
            gameFinished = true;
        }
    }

    public void enableObjectiveReminder()
    {
        objectiveReminder.enabled = true;
    }


}