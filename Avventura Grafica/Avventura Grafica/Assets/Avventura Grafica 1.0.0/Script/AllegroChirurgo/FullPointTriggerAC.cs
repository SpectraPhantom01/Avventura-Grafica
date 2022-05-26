using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullPointTriggerAC : MonoBehaviour
{
    [Header("Oggetto Padre")]
    [SerializeField] ParteCorpoMancanteAllegroChirurgo oggettoPadre;
    [Header("Curva di punteggio")]
    [SerializeField] AnimationCurve curve;
    [Header("Sensibilità accettata per FullPoint (suggerita 75)")]
    [SerializeField] float acceptableSensibilityScore;
    [Header("Bonus score")]
    [SerializeField] bool gotBonus = false;
    [SerializeField] float bonusScore;
    [Header("Level Step Score must be 5!!! 0 = BEST SCORE")]
    [SerializeField] List<float> scoreList;


    float positionSensibility;
    float percentageScore;

    ParteCorpoAllegroChirurgo parteAggiunta;
    GameManagerAllegroChirurgo gameManagerAC;
    bool firstCheckAvailable = true;
    bool gotFullPoint = false;
    private void Start()
    {
        percentageScore = 0;
    }

    private void Update()
    {
        if(parteAggiunta != null)
        {
            if(!parteAggiunta.GetState()) // <<----- se non è più selezionata
            {
                if (firstCheckAvailable)
                {
                    CheckAllignment();
                    firstCheckAvailable = false;
                }
            }
        }
    }

    internal void SetParteDelCorpo(ParteCorpoAllegroChirurgo parteCorpoAllegroChirurgo)
    {
        parteAggiunta = parteCorpoAllegroChirurgo;
    }

    public void InitializeMe(float positionSens, GameManagerAllegroChirurgo gmAc)
    {
        positionSensibility = positionSens;
        //Keyframe k = new Keyframe(positionSensibility, 0);
        //k.inTangent = -positionSensibility;
        //curve.MoveKey(1, k);
        gameManagerAC = gmAc;
    }

    private void CheckAllignment()
    {

        float distanza = Vector3.Distance(gameObject.transform.position, parteAggiunta.transform.position);
        //  BISOGNA INVERTIRE IL VALORE DELLA DISTANZA CHE CHIARAMENTE DIMINUISCE CON L'AVVICINARSI AL PUNTO TRIGGER

        // se la posizione è dentro la sensibilità
        if (distanza < positionSensibility)
        {
            //distanza = Mathf.Abs(distanza - 1);

            float percent = Mathf.InverseLerp(positionSensibility, 0, distanza);
            print(percent);
            float scoringOnCurve = curve.Evaluate(percent);

            percentageScore = CalcolatePercentageOfScoring(scoringOnCurve, 100);

            if (percentageScore >= acceptableSensibilityScore)
                gotFullPoint = true;

            print(percentageScore);
        }
        else
        {
            // no points
            print("0 score");
            gotFullPoint = false;
        }
    }

    private float CalcolatePercentageOfScoring(float scoringOnCurve, float percent)
    {
        return percent * scoringOnCurve;
    }

    public void CheckNow()
    {
        firstCheckAvailable = true;
    }

    public float GetMyFinalScore()
    {
        if (gotFullPoint)
        {
            if (gotBonus)
                return scoreList[0] + bonusScore;
            else
                return scoreList[0];
        }
        else
        {
            return CalcolateScore();
        }
    }

    private float CalcolateScore()
    {
        float tempScore;

        if (percentageScore >= gameManagerAC.scaglione1)
            tempScore = scoreList[1];
        else if (percentageScore < gameManagerAC.scaglione1 && percentageScore >= gameManagerAC.scaglione2)
            tempScore = scoreList[2];
        else if (percentageScore < gameManagerAC.scaglione2 && percentageScore >= gameManagerAC.scaglione3)
            tempScore = scoreList[3];
        else if (percentageScore < gameManagerAC.scaglione3 && percentageScore >= gameManagerAC.scaglione4)
            tempScore = scoreList[4];
        else
            tempScore = 0;


        if (gotBonus)
            return bonusScore + tempScore;
        else
            return tempScore;
    }
}
