using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineaDiFondo : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<LetteraManager>() != null)
            collision.gameObject.GetComponent<LetteraManager>().BloccoPerso();
    }
}
