using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PointGiardinaggio : MonoBehaviour
{
    [SerializeField] Foglia fogliaPrefab;
    [SerializeField] int quantitaFoglie;
    [SerializeField] float radius;
    bool colpito = false;
    public bool imLast { get; private set; } = false;

    int myId;
    List<Foglia> foglie = new List<Foglia>();

    private void Start()
    {
        for (int i = 0; i < quantitaFoglie; i++)
        {
            Vector3 rndPos = UnityEngine.Random.insideUnitCircle * radius;
            foglie.Add(Instantiate(fogliaPrefab, transform.position + rndPos, Quaternion.identity));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!colpito)
        {
            DrawLine line = collision.gameObject.GetComponent<DrawLine>();
            if (line != null)
            {
                colpito = true;
                CheckPoint father = GetComponentInParent<CheckPoint>();
                father.IWasTheLastHit(this);

                foglie.ForEach(x => x.Hit());
            }
        }

    }

    internal void SetNewSprite(Sprite newSprite)
    {
        fogliaPrefab.spriteRenderer.sprite = newSprite;
    }

    public void ResetMe()
    {
        colpito = false;
    }

    public void SetMeAsLast(bool var)
    {
        imLast = var;
    }

    internal void FinishWithMe()
    {
        gameObject.GetComponentInParent<CheckPoint>().ItsOver();
    }

    internal void ChangeMyId(int i)
    {
        myId = i;
    }

    public int GetMyId()
    {
        return myId;
    }

    private void OnDestroy()
    {
        foreach(Foglia f in foglie)
        {
            if(f!= null)
            {
                Destroy(f.gameObject);
            }
        }
    }
}
