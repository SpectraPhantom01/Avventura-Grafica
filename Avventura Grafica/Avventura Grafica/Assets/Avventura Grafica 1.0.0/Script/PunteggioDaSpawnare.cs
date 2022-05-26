using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunteggioDaSpawnare : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void Inizializza(Vector3 pos)
    {
        gameObject.transform.position = pos;
        Invoke("Distruggimi", 1f);
    }

    public void Distruggimi()
    {
        Destroy(gameObject);
    }
}
