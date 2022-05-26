using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParteInferiore : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        PezzoInValigia pezzoColliso = collision.collider.GetComponentInParent<PezzoInValigia>();

        if(pezzoColliso != null)
        {
            PezzoInValigia father = GetComponentInParent<PezzoInValigia>();
            if (father.dropArea.IsSomeoneThere(father))
            {
                father.GoBackToDroppingPosition();
            }
        }
    }
}
