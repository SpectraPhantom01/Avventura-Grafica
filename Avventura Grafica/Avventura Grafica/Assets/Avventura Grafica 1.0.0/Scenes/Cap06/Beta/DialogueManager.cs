using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AC;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] string nomeVariabile;
    [SerializeField] GameObject listaNpc;
    int numeroNpc;

    [SerializeField] Cutscene cutscene;
    GVar variabile;

    private void Start()
    {
        variabile = GlobalVariables.GetVariable(nomeVariabile);
        numeroNpc = listaNpc.transform.childCount;
    }

    void Update()
    {
        Controllo();
    }

    private void Controllo()
    {
        if (numeroNpc == 0 && variabile.BooleanValue != true)
        {
            variabile.BooleanValue = true;
            cutscene.Interact();
        }
    }

    public void ScalaContatore()
    {
        numeroNpc--;
    }


}
