using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeFigure : MonoBehaviour
{
    [SerializeField] bool interno;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        DrawLine line = collision.gameObject.GetComponent<DrawLine>();
        if (line != null)
        {
            if(interno)
                line.GetComponentInParent<LineDrawer>().EndDraw(true);
                        
        }
    }
    
}
