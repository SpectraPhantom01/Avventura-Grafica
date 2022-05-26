using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public TiroAlBersaglioManager managerTAB;
    public GameManagerAllegroChirurgo managerAC;

    public void StartTimer(int tempoMassimo)
    {
        StartCoroutine(IniziaTimer(tempoMassimo));
    }

    private IEnumerator IniziaTimer(int tempoMassimo)
    {
        int tempoPassato = 0;
        while (tempoPassato <= tempoMassimo)
        {
            gameObject.GetComponent<TextMeshProUGUI>().text = (tempoMassimo - tempoPassato).ToString();
            yield return new WaitForSeconds(1);
            tempoPassato++;
        }

        if(managerTAB != null)
            managerTAB.TimerFinito();

        if (managerAC != null)
            managerAC.TimerFinito();

        
    }

    internal void StopTimer(int tempo)
    {
        StopAllCoroutines();
        gameObject.GetComponent<TextMeshProUGUI>().text = "";
    }
}
