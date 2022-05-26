using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorreDifesa : MonoBehaviour
{
    [SerializeField] List<Sprite> vitaList;
    public int vita { get; private set; } = 10;
    
    SpriteRenderer sRenderer;
    public void InizializzaTorre(int index, Transform pivot)
    {
        switch(index)
        {
            case 0:
                gameObject.transform.position = new Vector2(pivot.position.x, pivot.position.y);
                break;
            case 1:
                gameObject.transform.position = new Vector2(pivot.position.x + 12f, pivot.position.y);
                break;
            case 2:
                gameObject.transform.position = new Vector2(pivot.position.x + 24f, pivot.position.y);
                break;
        }
        vita = vitaList.Count - 1;
        sRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<ProiettileSpaceInvaders>() != null)
            Danneggiami(1);
        if (collision.gameObject.GetComponent<NemicoSpaceInvaders>() != null)
            Destroy(gameObject);
    }

    public void Danneggiami(int quantita)
    {
        vita -= quantita;
        if (vita < 0)
            Destroy(gameObject);
        else
            SettaHP();
    }

    private void SettaHP()
    {
        sRenderer.sprite = vitaList[vita];
    }

    
}
