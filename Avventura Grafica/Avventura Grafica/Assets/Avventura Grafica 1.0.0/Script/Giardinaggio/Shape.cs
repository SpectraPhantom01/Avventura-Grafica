using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : MonoBehaviour
{
    [SerializeField] Sprite fogliaSprite;
    [SerializeField] CheckPoint checkPoint;
    [Header("Immagine d'aiuto a tempo")]
    [SerializeField] SpriteRenderer shapeImage;
    [Tooltip("Se il tempo è maggiore di 0, allora l'immagine sparirà dopo il tempo impostato.")]
    [SerializeField] float timedShape;
    public GameObject outLineFoglie;

    List<Foglia> foglieEsterne = new List<Foglia>();
    bool playTimer;
    float _timePassed;
    bool hidden = false;
    private void Start()
    {
        checkPoint.Initialize(fogliaSprite);
        GetFoglieEsterne();
    }

    private void Update()
    {
        if(playTimer && !hidden)
        {
            _timePassed += Time.deltaTime;
            if(_timePassed >= timedShape)
            {
                _timePassed = 0;
                hidden = true;
                ChangeSpriteState(false);
            }
        }
    }

    public void ChangeSpriteState(bool enabled)
    {
        if(playTimer && hidden)
            shapeImage.enabled = enabled;
    }

    private void GetFoglieEsterne()
    {
        foreach(Transform t in outLineFoglie.transform)
        {
            Foglia f = t.GetComponent<Foglia>();
            if (t.GetComponent<Foglia>().External(this))
            {
                foglieEsterne.Add(f);
            }

            f.spriteRenderer.sprite = fogliaSprite;
        }
    }

    public void ClearImage()
    {
        foreach(Foglia f in foglieEsterne)
        {
            f.Hit();
        }
        foglieEsterne.Clear();
    }

    internal void RemoveLeafFromList(Foglia foglia)
    {
        foglieEsterne.Remove(foglia);
    }

    public void ShapeTimer()
    {
        if(timedShape > 0)
        {
            playTimer = true;
        }
    }
}
