using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CambiaOrario : MonoBehaviour
{
    
    public void Cambia(string newTime)
    {
        TextMeshPro testo = gameObject.GetComponentInChildren<TextMeshPro>();

        if(testo != null)
        {
            testo.text = newTime;
        }

    }
}
