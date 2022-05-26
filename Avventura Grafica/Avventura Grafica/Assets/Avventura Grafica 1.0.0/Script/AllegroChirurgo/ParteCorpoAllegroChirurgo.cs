using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParteCorpoAllegroChirurgo : MonoBehaviour
{
    [SerializeField] ParteDelCorpoAC parteDelCorpo;
    [Header("Rif obj from scene")]
    [SerializeField] FullPointTriggerAC destination;
    bool selected;

    private void Start()
    {
        SetDestination();
    }

    private void SetDestination()
    {
        if(destination != null)
            destination.SetParteDelCorpo(this);
    }

    public ParteDelCorpoAC GetParteDelCorpo()
    {
        return parteDelCorpo;
    }

    private void Update()
    {
        if(selected)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position += new Vector3(0, 0, 10);
        }
    }

    internal bool GetState()
    {
        return selected;
    }

    internal void Selected(bool v)
    {
        selected = v;
        if (v == false && destination != null)
            destination.CheckNow();
    }

    
}
