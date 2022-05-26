using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chirurgo : MonoBehaviour
{
    // -------------------  PLAYER's Inputs  ------------------------


    [SerializeField] Camera cameraObj;
    [SerializeField] float speedRotationObjectSelected;
    ParteCorpoAllegroChirurgo pezzoSelezionato;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (pezzoSelezionato == null)
                ShootRay();
            else
            {
                pezzoSelezionato.Selected(false);
                pezzoSelezionato = null;
            }
        }
        
    }

    private void ShootRay()
    {
        bool hoColpitoQualcosa;

        RaycastHit2D informazioniSulRaggio = Physics2D.CircleCast(cameraObj.ScreenToWorldPoint(Input.mousePosition), 0.01f, Vector2.up, 0);

        hoColpitoQualcosa = informazioniSulRaggio.collider != null;

        if (hoColpitoQualcosa)
        {
            pezzoSelezionato = informazioniSulRaggio.collider.gameObject.GetComponent<ParteCorpoAllegroChirurgo>();
            if (pezzoSelezionato != null)
            {

                pezzoSelezionato.Selected(true);

            }

        }
    }
}
