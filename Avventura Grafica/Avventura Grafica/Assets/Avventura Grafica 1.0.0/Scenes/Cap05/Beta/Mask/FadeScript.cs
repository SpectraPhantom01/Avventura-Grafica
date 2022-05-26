using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeScript : MonoBehaviour
{
    [SerializeField] float timeValueFadeOut = 0.4f;
    [SerializeField] float timeValueFadeIn = 0.8f;
    [SerializeField] float valueFadeOut = 0.5f;
    [SerializeField] float valueFadeIn = 0;

    Color maskColor;

    private void Start()
    {
        maskColor = GetComponent<SpriteRenderer>().material.color;
    }

    public void FadeActive()
    {
        StartCoroutine(ActiveCoroutine());
    }

    public void FadeInactive()
    {
        StartCoroutine(InactiveCoroutine());
    }

    public IEnumerator ActiveCoroutine()
    {
        while (maskColor.a <= valueFadeOut)
        {
            maskColor.a += (timeValueFadeOut * Time.deltaTime);
            GetComponent<SpriteRenderer>().material.SetColor("_Color", new Color(1, 1, 1, maskColor.a));
            yield return null;
        }
    }

    public IEnumerator InactiveCoroutine()
    {
        while (maskColor.a >= valueFadeIn)
        {
            maskColor.a -= (timeValueFadeIn * Time.deltaTime);
            GetComponent<SpriteRenderer>().material.SetColor("_Color", new Color(1, 1, 1, maskColor.a));
            yield return null;
        }
    }
}
