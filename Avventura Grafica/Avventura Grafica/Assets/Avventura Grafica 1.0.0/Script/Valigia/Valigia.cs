using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Valigia : MonoBehaviour
{
    [SerializeField] GameManagerMGValigia gmValigia;
    public List<PezzoInValigia> listaPezziDentro { get; private set; }

    private void Start()
    {
        listaPezziDentro = new List<PezzoInValigia>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PezzoInValigia pezzo = collision.GetComponent<PezzoInValigia>();
        if(pezzo != null)
        {
            pezzo.ImInside(true);
            listaPezziDentro.Add(pezzo);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PezzoInValigia pezzo = collision.GetComponent<PezzoInValigia>();
        if (pezzo != null)
        {
            pezzo.ImInside(false);
            listaPezziDentro.Remove(pezzo);
        }
    }

    public int GetNecessaryPieces()
    {
        return listaPezziDentro.FindAll(x => x.necessario == true).Count;
    }
}
