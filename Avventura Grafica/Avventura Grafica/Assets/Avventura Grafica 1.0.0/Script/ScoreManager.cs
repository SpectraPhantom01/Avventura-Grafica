using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum Livello { livello1, livello2, livello3, livello4, livello5, livello6, livello7, livello8, livello9, livello10, livello11, vittoria}

public class ScoreManager : MonoBehaviour
{
    // riferimenti alla scena
    [SerializeField] GameObject ScalataManagerObj;
    [SerializeField] UnityEvent onNextLevel;
    [SerializeField] UnityEvent onGameOver;
    [SerializeField] UnityEvent onWin;
    ScalataManager scM;
    
    // variabili da inspector
    // step
    [SerializeField] int stepLivello1;
    [SerializeField] int stepLivello2;
    [SerializeField] int stepLivello3;
    [SerializeField] int stepLivello4;
    [SerializeField] int stepLivello5;
    [SerializeField] int stepLivello6;
    [SerializeField] int stepLivello7;
    [SerializeField] int stepLivello8;
    [SerializeField] int stepLivello9;
    [SerializeField] int stepLivello10;
    [SerializeField] int stepFinale;
    // velocità discesa blocchi
    [SerializeField] float velocitaDiscesaBlocchi;
    // velocità spawn
    [SerializeField] float quantitaDecrementaSecondi;
    // limite minimo sotto il qale la partita viene persa
    [SerializeField] float limiteGameOver;
    [SerializeField] float limiteBlocchiPersi;

    Livello livelloAttuale;
    int scoreAttuale;
    int blocchiPersi;
    // Start is called before the first frame update
    void Start()
    {
        ResetScore();
    }

    // Update is called once per frame
    void Update()
    {
        scoreAttuale = scM.GetScore();
        blocchiPersi = scM.GetBlocchiPersi();
        string messaggioScore = "Score: " + scoreAttuale.ToString() + " Miss: " + blocchiPersi.ToString();
        scM.AggiornaTestoLivelloOScore(messaggioScore, false);
        if (scoreAttuale > 0)
        {
            switch (livelloAttuale)
            {
                case Livello.livello1:
                    ControllaScore(scoreAttuale, stepLivello1, Livello.livello1, Livello.livello2);
                    break;
                case Livello.livello2:
                    ControllaScore(scoreAttuale, stepLivello2, Livello.livello2, Livello.livello3);
                    break;
                case Livello.livello3:
                    ControllaScore(scoreAttuale, stepLivello3, Livello.livello3, Livello.livello4);
                    break;
                case Livello.livello4:
                    ControllaScore(scoreAttuale, stepLivello4, Livello.livello4, Livello.livello5);
                    break;
                case Livello.livello5:
                    ControllaScore(scoreAttuale, stepLivello5, Livello.livello5, Livello.livello6);
                    break;
                case Livello.livello6:
                    ControllaScore(scoreAttuale, stepLivello6, Livello.livello6, Livello.livello7);
                    break;
                case Livello.livello7:
                    ControllaScore(scoreAttuale, stepLivello7, Livello.livello7, Livello.livello8);
                    break;
                case Livello.livello8:
                    ControllaScore(scoreAttuale, stepLivello8, Livello.livello8, Livello.livello9);
                    break;
                case Livello.livello9:
                    ControllaScore(scoreAttuale, stepLivello9, Livello.livello9, Livello.livello10);
                    break;
                case Livello.livello10:
                    ControllaScore(scoreAttuale, stepLivello10, Livello.livello10, Livello.livello11);
                    break;
                case Livello.livello11:
                    ControllaScore(scoreAttuale, stepFinale, Livello.livello11, Livello.vittoria);
                    break;
                case Livello.vittoria:
                    Vittoria();
                    break;

            }
        }
        if(scoreAttuale <= limiteGameOver || blocchiPersi >= limiteBlocchiPersi)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        scM.AggiornaTestoLivelloOScore("HAI PERSO!!!", true);
        onGameOver.Invoke();
        scM.GiocoFinito();
    }

    private void Vittoria()
    {
        onWin.Invoke();
        scM.GiocoFinito();
    }

    private void ControllaScore(int score, int step, Livello livelloDaSuperare, Livello livelloSuccessivo)
    {
        if(score >= step && livelloAttuale == livelloDaSuperare)
        {
            CambiaLivello(livelloSuccessivo);
            scM.AggiornaTestoLivelloOScore(LivelloAttualeToString(), true);
            scM.DecrementaSecondiSpawn(quantitaDecrementaSecondi);
            scM.AumentaVelocitaDiscesa(velocitaDiscesaBlocchi);
        }
    }

    private string LivelloAttualeToString()
    {
        switch (livelloAttuale)
        {
            case Livello.livello1:
                return "Livello 1 (" + stepLivello1 +")";
            case Livello.livello2:
                return "Livello 2 (" + stepLivello2 + ")";
            case Livello.livello3:
                return "Livello 3 (" + stepLivello3 + ")";
            case Livello.livello4:
                return "Livello 4 (" + stepLivello4 + ")";
            case Livello.livello5:
                return "Livello 5 (" + stepLivello5 + ")";
            case Livello.livello6:
                return "Livello 6 (" + stepLivello6 + ")";
            case Livello.livello7:
                return "Livello 7 (" + stepLivello7 + ")";
            case Livello.livello8:
                return "Livello 8 (" + stepLivello8 + ")";
            case Livello.livello9:
                return "Livello 9 (" + stepLivello9 + ")";
            case Livello.livello10:
                return "Livello 10 (" + stepLivello10 + ")";
            case Livello.livello11:
                return "Livello 11 (" + stepFinale + ")";
            case Livello.vittoria:
                return "VITTORIA!!!";
            default:
                return "";
        }
    }

    private void CambiaLivello(Livello nuovoLivello)
    {
        livelloAttuale = nuovoLivello;
        NextLevelEvent();
    }

    public void ResetScore()
    {
        scM = ScalataManagerObj.GetComponent<ScalataManager>();
        scoreAttuale = scM.GetScore();
        blocchiPersi = scM.GetBlocchiPersi();
        livelloAttuale = Livello.livello1;
        scM.AggiornaTestoLivelloOScore(LivelloAttualeToString(), true);
    }

    public void NextLevelEvent()
    {
        onNextLevel.Invoke();
    }
}
