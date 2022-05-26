using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public enum TipoScatolone { Piccolo, Medio, Grande }

public class GameManagerMGValigia : MonoBehaviour
{
    static GameManagerMGValigia _instance;
    public static GameManagerMGValigia Instance => _instance;

    List<PezzoInValigia> listaPezzi = new List<PezzoInValigia>();

    [SerializeField] DroppingArea dropArea;

    [SerializeField] Valigia valigia;

    [Header("Area pezzi in inventario cam valigia")]
    [SerializeField] Transform upLeftCorner;
    [SerializeField] Transform lowRightCorner;

    [Header("Finale")]
    [SerializeField] UnityEvent onExitAvailable;


    [SerializeField] UnityEvent onExitNotAvailable;

    PezzoInValigia currentObject;

    int countNecessari;

    bool canDrop = true;
    bool watchingValigia = false;

    private void Awake()
    {
        _instance = this;
    }

    internal void Register(PezzoInValigia pezzoInValigia)
    {
        listaPezzi.Add(pezzoInValigia);
        countNecessari += pezzoInValigia.necessario ? 1 : 0;
    }

    // Update is called once per frame
    void Update()
    {
        InputCatcher();
    }

    private void InputCatcher()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (currentObject == null && canDrop)
                LanciaRaggio();
            else
                LasciaOggetto();
        }

    }

    private void LasciaOggetto()
    {
        if (currentObject != null)
        {
            currentObject.FinishPicking();
            currentObject = null;
            canDrop = false;
        }
    }

    public void CheckIfIsAllOk() // da chiamare attraverso l'action che permette di uscire dalla scena (o di chiudere la valigia)
    {
        if(valigia.GetNecessaryPieces() == countNecessari)
        {
            print("finito, uscita");
            onExitAvailable.Invoke();
        }
        else
        {
            print("non ancora finito");
            onExitNotAvailable.Invoke();
        }
    }

    private void LanciaRaggio()
    {


        bool gotSomething;

        RaycastHit2D info = Physics2D.CircleCast(Camera.main.ScreenToWorldPoint(Input.mousePosition), 0.01f, Vector2.up, 0); // usa layermask

        gotSomething = info.collider != null;

        if (gotSomething)
        {
            PezzoInValigia pezzoColpito = info.collider.gameObject.GetComponent<PezzoInValigia>();
            if (pezzoColpito != null)
            {

                if (watchingValigia)
                {
                    if (pezzoColpito.TryToGetMe())
                    {
                        currentObject = pezzoColpito;
                        currentObject.GoToDroppingPosition(dropArea.pivotDropping.position, dropArea, this);
                    }
                }
                else
                    pezzoColpito.PutMeInInventory(upLeftCorner.position, lowRightCorner.position);
                
            }
        }
    }

    public void WatchingValigia(bool state)
    {
        watchingValigia = state;
    }

    internal void YouCanDropAgain()
    {
        canDrop = true;
    }

    internal void BackOnPicking(PezzoInValigia pezzoInValigia)
    {
        currentObject = pezzoInValigia;
    }
}

