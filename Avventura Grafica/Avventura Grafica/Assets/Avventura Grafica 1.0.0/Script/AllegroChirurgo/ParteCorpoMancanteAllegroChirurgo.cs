using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParteCorpoMancanteAllegroChirurgo : MonoBehaviour
{
    [SerializeField] ParteDelCorpoAC parteDelCorpo;

    public ParteDelCorpoAC GetParteDelCorpo()
    {
        return parteDelCorpo;
    }
}
