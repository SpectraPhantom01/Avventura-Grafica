using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public EdgeCollider2D edgeCollider;

    float circleColliderRadius;

    public List<Vector2> points = new List<Vector2>();

    public float pointsMinDistance = 0.1f;

    List<Sprite> sprites;
    SpriteRenderer spriteToChange;
    int contatore = 0;
    int contaSprite = 0;
    private void Update()
    {
        if(contatore == 5)
        {
            contatore = 0;
            AnimateCursor(SelectSprite());
        }
    }

    private int SelectSprite()
    {
        if(contaSprite == 0)
        {
            contaSprite++;
            return 1;
        }
        else
        {
            contaSprite = 0;
            return 0;
        }
    }

    public void AddPoint(Vector2 newPoint)
    {
        if(points.Count >= 1 && Vector2.Distance(newPoint, GetLastPoint()) < pointsMinDistance)
        {
            return;
        }

        points.Add(newPoint);

        CircleCollider2D circleCollider = this.gameObject.AddComponent<CircleCollider2D>();
        circleCollider.offset = newPoint;
        circleCollider.radius = circleColliderRadius;

        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPosition(points.Count - 1, newPoint);

        if (points.Count > 1)
            edgeCollider.points = points.ToArray();

        contatore++;
    }

    public Vector2 GetLastPoint()
    {
        return lineRenderer.GetPosition(points.Count - 1);
    }

    public void SetLineColor(Gradient colorGradient)
    {
        lineRenderer.colorGradient = colorGradient;
    }

    public void SetPointMinDistance(float distance)
    {
        pointsMinDistance = distance;
    }

    public void SetLineWidth(float width)
    {
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;

        edgeCollider.edgeRadius = width / 2f;
        circleColliderRadius = width / 2f;

    }

    internal void GiveNewCursor(List<Sprite> spritesPuntatore, SpriteRenderer puntatoreObject)
    {

        sprites = spritesPuntatore;
        spriteToChange = puntatoreObject;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PointGiardinaggio punto = collision.gameObject.GetComponent<PointGiardinaggio>();
        if (punto != null && punto.imLast)
        {
            punto.FinishWithMe();
        }
    }

    void AnimateCursor(int index)
    {
        spriteToChange.sprite = sprites[index];
    }
}

