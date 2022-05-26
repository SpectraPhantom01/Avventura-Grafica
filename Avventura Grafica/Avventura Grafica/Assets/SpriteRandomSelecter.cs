using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRandomSelecter : MonoBehaviour
{
    public List<Sprite> listaSprite;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = listaSprite[UnityEngine.Random.Range(0, listaSprite.Count - 1)];
    }

}
