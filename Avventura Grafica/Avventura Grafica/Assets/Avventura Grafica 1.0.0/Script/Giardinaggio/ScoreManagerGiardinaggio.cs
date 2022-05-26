using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ScoreManagerGiardinaggio : MonoBehaviour
{
    float score = 0;
    float scoreGoal;
    [SerializeField] GameManagerGiardinaggio gmGiardinaggio;

    [Header("Scrivi numeri")]
    [SerializeField] float minimumPercetange;
    [SerializeField] TextMeshProUGUI scoreText;

    CheckPoint currentCeckpoints;
    List<int> scoreOnlevels = new List<int>();
    public List<int> ScoresOnLevels => scoreOnlevels;
    private void Start()
    {
        minimumPercetange /= 100;
    }

    public void ResetScore(float newScoreGoal)
    {
        score = 0;
        scoreGoal = newScoreGoal;
        scoreText.text = "";
    }

    public void ResetScore()
    {
        score = 0;
        scoreText.text = "";
    }

    public void AddScore()
    {
        score += 1;
        
    }

    public void PrintScore()
    {
        int finalScore = GetRealScore();
        scoreOnlevels.Add(finalScore);
        scoreText.text = "Punteggio: " + finalScore.ToString();
    }

    private int GetRealScore()
    {
        if (score == 0)
            return 0;

        float scoring;
        float levelPercentage;
        float timerScore;
        float beginningTimer = gmGiardinaggio.timerManager.BeginningTime;
        float endingTimer = gmGiardinaggio.timerManager.GetTimerValue();
        int bonusLifes = gmGiardinaggio.lifeManager.CurrentLifes * 250;

        levelPercentage = (score / scoreGoal) * 1000;
        timerScore = (endingTimer / beginningTimer) * 100;

        scoring = levelPercentage + timerScore + bonusLifes;

        return (int)scoring;
    }

    private void Update()
    {
        if(score == scoreGoal)
        {
            if(!gmGiardinaggio.GamePaused)
                gmGiardinaggio.EndShape(true);
        }
    }

    internal bool EnoughPercentage()
    {
        float a = (scoreGoal - score) / scoreGoal;
        a = 1 - a;
        print("percentuale: " + a);
        return a > minimumPercetange;
    }

    internal void GiveMeCurrentCheckpoints(CheckPoint newCheckPoint)
    {
        currentCeckpoints = newCheckPoint;
    }
}
