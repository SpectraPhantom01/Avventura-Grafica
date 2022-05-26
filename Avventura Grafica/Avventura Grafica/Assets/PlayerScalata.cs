using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScalata : MonoBehaviour
{
    [SerializeField] List<Transform> listaPivot;
    [SerializeField] float speed;
    SpriteRenderer spRenderer;
    bool active = false;
    int contatore = 0;
    private void FixedUpdate()
    {
        if(active)
        {
            if(Vector2.Distance(transform.position, NextPoint()) > 0.1f)
                GoToNextPoint();
            else
            {
                active = false;
                transform.position = NextPoint();
                FlipSprite();
                contatore++;
            }
        }
    }

    private void GoToNextPoint()
    {
        transform.position = Vector2.Lerp(transform.position, NextPoint(), speed * Time.fixedDeltaTime);

    }

    private Vector2 NextPoint()
    {
        return listaPivot[contatore].position;
    }

    private void Start()
    {
        spRenderer = GetComponent<SpriteRenderer>();
    }

    public void FlipSprite()
    {
        if(spRenderer.flipX)
        {
            spRenderer.flipX = false;
        }
        else
        {
            spRenderer.flipX = true;
        }
    }

    public void ActivateMe()
    {
        active = true;
    }
}
