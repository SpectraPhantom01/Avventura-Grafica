using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerCountdown : MonoBehaviour
{
    // public variables
    public bool timerIniziato;
    public bool timerDaDistruggere;
    public float secondi = 30f;
    public float tempoIntervallo;
    public GameObject oggettoDaVisualizzare;

    // private variables
    float tempoCheScorre;
    
    
    // Start is called before the first frame update
    void Start()
    {
        tempoCheScorre = 0;

    }

    // Update is called once per frame
    void Update()
    {
        if (timerIniziato)
        {
            if (tempoCheScorre <= secondi)
            {
                tempoCheScorre += 1 * Time.deltaTime;
            }
            else
            {
                if (oggettoDaVisualizzare.GetComponent<SpriteRenderer>() != null)
                {
                    oggettoDaVisualizzare.GetComponent<SpriteRenderer>().enabled = true;
                    if (timerDaDistruggere)
                        Destroy(gameObject);
                }else if(oggettoDaVisualizzare.GetComponent<Image>() != null)
                {
                    oggettoDaVisualizzare.GetComponent<Image>().enabled = true;
                    if (timerDaDistruggere)
                        Destroy(gameObject);
                }
            }
            if (tempoCheScorre > secondi && timerDaDistruggere == false)
            {
                tempoCheScorre += 1 * Time.deltaTime;
                if (tempoCheScorre > tempoIntervallo)
                {
                    if (oggettoDaVisualizzare.GetComponent<SpriteRenderer>() != null)
                    {
                        tempoCheScorre = 0;
                        oggettoDaVisualizzare.GetComponent<SpriteRenderer>().enabled = false;
                    }
                    else if (oggettoDaVisualizzare.GetComponent<Image>() != null)
                    {
                        tempoCheScorre = 0;
                        oggettoDaVisualizzare.GetComponent<Image>().enabled = false;

                    }
                }
            }
        }
    }

    public void IniziaTimer()
    {
        timerIniziato = true;
    }

}
