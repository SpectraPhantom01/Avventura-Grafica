using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LineDrawer : MonoBehaviour
{
    [Header("Sprite che cambiano")]
    [SerializeField] List<Sprite> spritesPuntatore;
    [SerializeField] GameObject puntatoreObject;
    [SerializeField] GameManagerGiardinaggio gameManager;

    public GameObject linePrefab;
    public LayerMask stopDrawing;
    int stopDrawingIndex;

    [Space(30f)]
    public Gradient lineColor;
    public float linePointsMinDistance;
    public float lineWidth;

    DrawLine currentLine;
    bool drawActive;
    Camera cam;

    public UnityEvent onMistake;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        stopDrawingIndex = LayerMask.NameToLayer("StopDrawingMGGiard");
    }

    // Update is called once per frame
    void Update()
    {
        if (drawActive)
        {
            if (Input.GetMouseButtonDown(0))
            {
                BeginDraw();
            }

            if (currentLine != null)
            {
                Draw();
            }

            if (Input.GetMouseButtonUp(0))
            {
                EndDraw(false);
            }
        }
    }

    internal void CanDraw(bool v)
    {
        drawActive = v;
    }

    void BeginDraw()
    {
        currentLine = Instantiate(linePrefab, transform).GetComponent<DrawLine>();

        SpriteRenderer sprite = puntatoreObject.GetComponentInChildren<SpriteRenderer>();
        currentLine.GiveNewCursor(spritesPuntatore, sprite);
        currentLine.SetLineColor(lineColor);
        currentLine.SetPointMinDistance(linePointsMinDistance);
        currentLine.SetLineWidth(lineWidth);
    }

    void Draw()
    {
        Vector2 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);

        currentLine.AddPoint(mousePosition);
    }

    public void EndDraw(bool mistakeFromEdge)
    {
        if(currentLine != null)
        {
            if(currentLine.points.Count < 2)
            {
                Destroy(currentLine.gameObject);
            }
            else
            {
                currentLine = null;
                if (mistakeFromEdge)
                    Mistake();
            }
        }
    }

    public void ResetLines()
    {
        foreach(Transform t in gameObject.transform)
        {
            Destroy(t.gameObject);
        }
    }

    public void Mistake()
    {
        onMistake.Invoke();
    }
}
