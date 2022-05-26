using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBackgroundScalata : MonoBehaviour
{
    [SerializeField] float distanzaDaPercorrere;
    [Header("è corretto avere la stessa velocità del player")]
    [SerializeField] float speed;
    bool active;
    Vector2 oldPos;

    private void MoveBackground()
    {
        transform.position = Vector2.Lerp(transform.position, NextPoint(), speed * Time.deltaTime);
    }

    private void Start()
    {
        SetOldPos();
    }

    private void SetOldPos()
    {
        oldPos = transform.position;
    }

    private void Update()
    {
        if(active)
        {
            if (Vector2.Distance(transform.position, NextPoint()) > 0.1f)
                MoveBackground();
            else
            {
                active = false;
                transform.position = NextPoint();
                SetOldPos();
            }
        }
    }

    private Vector2 NextPoint()
    {
        return new Vector2(oldPos.x, oldPos.y - distanzaDaPercorrere);
    }

    public void ActivateMe()
    {
        active = true;
    }
}
