using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBack : MonoBehaviour
{

    public void SpostaDietroDiCento()
    {
        gameObject.GetComponent<SpriteRenderer>().sortingOrder -= 100;
    }

}
