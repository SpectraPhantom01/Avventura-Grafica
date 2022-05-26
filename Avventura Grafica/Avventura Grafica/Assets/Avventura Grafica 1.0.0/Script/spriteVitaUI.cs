using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spriteVitaUI : MonoBehaviour
{
    public void InizializzaVita(int y, RectTransform pivotViteUI)
    {
        RectTransform rc = gameObject.GetComponent<RectTransform>();
        rc.position = new Vector2(pivotViteUI.position.x, pivotViteUI.position.y + y * 33f);
    }

    public void GetVita()
    {
        Destroy(gameObject);
    }
}
