using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum ParteDelCorpoAC { Bicipite, Clavicola, Deltoide, Femore, Gastrocnemio, GranDorsale, Radio, Rotula, Sartorio, Sterno, Tibia, Trapezio, Altro, SirDan}

public class GameManagerAllegroChirurgo : MonoBehaviour
{
    public static GameManagerAllegroChirurgo Instance;

    [Header("Percentuale minima per scaglione")]
    public int scaglione1;
    public int scaglione2;
    public int scaglione3;
    public int scaglione4;
    [Header("Lista di tutti i punti trigger")]
    [SerializeField] List<FullPointTriggerAC> fullPointList;
    [Header("Lista di tutte le parti del corpo")]
    [SerializeField] List<ParteCorpoAllegroChirurgo> partiCorpoList;
    [Header("Sensibilità del trigger")]
    [SerializeField] float positionSensibility = 0.2f;
    [Header("Timer displayed")]
    [SerializeField] Timer timer;
    [SerializeField] int time;
    [Header("Canvas object for final score")]
    [SerializeField] Canvas canvasObj;

    int pezziPronti;
    int pezziTotali;
    bool timerStarted;

    List<ObjectPool> listaOggettiLanciati = new List<ObjectPool>();

    float totalFinalScore;

    private void Awake()
    {
        pezziTotali = 0;
        Instance = this;
        InitializeLists();
        pezziPronti = 0;
        timerStarted = false;
    }

    private void Update()
    {
        if(pezziPronti == pezziTotali && !timerStarted)
        {
            listaOggettiLanciati.ForEach(x => x.EnadleCollider());
            timer.StartTimer(time);
            timerStarted = true;
        }
    }

    private void InitializeLists()
    {
        fullPointList.ForEach(x => x.InitializeMe(positionSensibility, this));
    }

    internal void TimerFinito()
    {
        foreach(FullPointTriggerAC f in fullPointList)
        {
            totalFinalScore += f.GetMyFinalScore();
        }

        DisplayFinalScore(totalFinalScore);
    }

    private void DisplayFinalScore(float totalFinalScore)
    {
        canvasObj.GetComponentInChildren<TextMeshProUGUI>().text = "Punteggio Finale: " + totalFinalScore;
    }

    public void PezzoPronto()
    {
        pezziPronti++;
    }
        
    public void AddOneToPezziTotali(ObjectPool objPooled)
    {
        listaOggettiLanciati.Add(objPooled);
        pezziTotali++;
    }
}
