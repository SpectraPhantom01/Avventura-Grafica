using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppingArea : MonoBehaviour
{
    [SerializeField] GameManagerMGValigia gmValigia;
    public Transform pivotDropping;
    public Transform leftEdge;
    public Transform rightEdge;

    PezzoInValigia currentPiece;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PezzoInValigia pezzo = collision.GetComponent<PezzoInValigia>();

        if (pezzo != null)
        {
            currentPiece = pezzo;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PezzoInValigia pezzo = collision.GetComponent<PezzoInValigia>();

        if(pezzo != null)
        {
            currentPiece = null;
            gmValigia.YouCanDropAgain();
        }
    }

    public bool IsSomeoneThere(PezzoInValigia pezzoDaControllare)
    {
        if (currentPiece == pezzoDaControllare)
            return true;
        return false;
    }
}
