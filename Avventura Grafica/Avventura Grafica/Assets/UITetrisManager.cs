using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITetrisManager : MonoBehaviour
{
    [Header("Riferimenti")]
    [SerializeField] GameObject startmenu;
    [SerializeField] TextMeshProUGUI difficultyText;
    [SerializeField] TextMeshProUGUI difficultyTextOnGame;
    [SerializeField] TextMeshProUGUI stepsTextOnGame;
    [SerializeField] TextMeshProUGUI scoreTextOnGame;
    [SerializeField] GameObject onGameOverPanel;
    [SerializeField] TextMeshProUGUI finalMessageText;
    [SerializeField] TextMeshProUGUI finalScoreText;
    [SerializeField] TextMeshProUGUI finalLevelText;

    [Header("Messaggi finali")]
    [SerializeField] string winMessage;
    [SerializeField] string loseMessage;

    private void Awake()
    {
        if (!startmenu.activeSelf) startmenu.SetActive(true);
    }


    public void OnEnd(bool win, int level, int score)
    {
        onGameOverPanel.SetActive(true);

        finalMessageText.text = win ? winMessage : loseMessage;

        finalLevelText.text = $"LIVELLO: {level}";
        finalScoreText.text = $"PUNTEGGIO: {score}";
    }

    public void NewDifficultyText(string newMessage)
    {
        difficultyText.text = newMessage;
        difficultyTextOnGame.text = newMessage;
    }

    public void UpdateScore(string newScore)
    {
        scoreTextOnGame.text = newScore;
    }

    public void UpdateStepText(string newStep)
    {
        stepsTextOnGame.text = newStep;
    }

}
