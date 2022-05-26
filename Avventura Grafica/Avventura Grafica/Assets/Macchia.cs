using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Macchia : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] float decreaserSpeed;
    Color decreaser;
    float startTransparency;
    private void Awake()
    {
        decreaser = spriteRenderer.color;
        startTransparency = decreaser.a;
    }

    private void Update()
    {
        if (startTransparency >= 0)
        {
            startTransparency -= decreaserSpeed * Time.deltaTime;
            decreaser.a = startTransparency;
            spriteRenderer.color = decreaser;
        }
        else
            Destroy(gameObject);
    }
}
