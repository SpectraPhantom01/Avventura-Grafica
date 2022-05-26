using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tasto : MonoBehaviour
{
    [SerializeField] SpriteRenderer graficaBackground;
    [SerializeField] Color coloreOnPress;

    Color oldColor;

    private void Awake()
    {
        oldColor = graficaBackground.color;
    }

    public void LightMeUp()
    {
        graficaBackground.color = coloreOnPress;
    }

    public void SwitchMeOff()
    {
        graficaBackground.color = oldColor;
    }
}
