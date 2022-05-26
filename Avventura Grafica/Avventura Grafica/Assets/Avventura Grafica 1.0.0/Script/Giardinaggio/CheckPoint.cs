using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    List<PointGiardinaggio> listaPunti = new List<PointGiardinaggio>();
    List<PointGiardinaggio> listaPuntiColpiti = new List<PointGiardinaggio>();
    public Direzione mouseDirection { get; private set; } = Direzione.none;
    GameManagerGiardinaggio gmGiardinaggio;

    public PointGiardinaggio lastHit { get; private set; }
    Sprite leafSprite;

    public void Initialize(Sprite newSpriteForLeaf)
    {
        leafSprite = newSpriteForLeaf;
        AddAllPointsAtList();
    }

    internal void IWasTheLastHit(PointGiardinaggio pointGiardinaggio)
    {
        listaPuntiColpiti.Add(pointGiardinaggio);
        lastHit = pointGiardinaggio;
        gmGiardinaggio.AddScore();
    }

    private void AddAllPointsAtList()
    {
        int i = 0;
        foreach (Transform t in transform)
        {
            PointGiardinaggio newPoint = t.GetComponent<PointGiardinaggio>();
            listaPunti.Add(newPoint);
            newPoint.ChangeMyId(i);
            newPoint.SetNewSprite(leafSprite);
            i++;
        }
    }

    internal void GiveManager(GameManagerGiardinaggio gameManagerGiardinaggio)
    {
        gmGiardinaggio = gameManagerGiardinaggio;
    }

    internal void ItsOver()
    {
        gmGiardinaggio.EndShape(false);
    }

}
