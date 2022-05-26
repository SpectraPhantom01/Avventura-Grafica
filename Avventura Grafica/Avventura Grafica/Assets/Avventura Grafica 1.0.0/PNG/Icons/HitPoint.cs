using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPoint : MonoBehaviour
{
    bool readyToBeClicked;
    LetteraManager bloccoColorato;
    public string nome;
    private void Awake()
    {
        readyToBeClicked = false;
    }

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bloccoColorato = collision.gameObject.GetComponent<LetteraManager>();
        if(bloccoColorato != null)
        {
            SetReadyToBeClicked(true);
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        bloccoColorato = collision.gameObject.GetComponent<LetteraManager>();
        if (bloccoColorato != null)
        {
            SetReadyToBeClicked(false);
        }
    }

    private void SetReadyToBeClicked(bool v)
    {
        readyToBeClicked = v;
    }

    public bool Click()
    {
        if(readyToBeClicked)
        {
            bloccoColorato.Distruggimi();
            return true;
        }
        else
        {
            return false;
        }
    }
}
