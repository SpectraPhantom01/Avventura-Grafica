using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LifeGiardinaggio : MonoBehaviour
{
    [SerializeField] int maxLifes;
    [SerializeField] GameObject lifeSpriteParent;
    [SerializeField] GameObject lifeSpritesPrefab;
    [SerializeField] float xOffset = 2;
    [SerializeField] float yOffset = 2;
    [SerializeField] UnityEvent onLoseLife;

    int currentLifes;
    public int CurrentLifes => currentLifes;
    GameManagerGiardinaggio gmGiardinaggio;
    List<spriteVitaUI> listaLifes = new List<spriteVitaUI>();
    private void Start()
    {
        ResetLifes();
        AddLifesToCanvas();
    }

    private void AddLifesToCanvas()
    {
        for (int i = 0; i < maxLifes; i++)
        {
            spriteVitaUI newLife = Instantiate(lifeSpritesPrefab, lifeSpriteParent.transform).GetComponent<spriteVitaUI>();
            newLife.gameObject.transform.position = Vector3.zero;
            newLife.transform.position = new Vector3(xOffset, (i + 1) * yOffset, 0);
            listaLifes.Add(newLife);
        }
    }

    private void Update()
    {
        if (currentLifes <= 0)
        {
            gmGiardinaggio.GameOver();
        }
    }

    public void Mistake()
    {
        DestroyOneLifeUI();
        onLoseLife.Invoke();
    }

    public void AddLifes(int num)
    {
        currentLifes += num;
    }
    public void RemoveOneLife()
    {
        currentLifes--;
    }
    public void ResetLifes()
    {
        currentLifes = maxLifes;
    }

    public void GetManager(GameManagerGiardinaggio gm)
    {
        gmGiardinaggio = gm;
    }

    public int GetInitialLifes()
    {
        return maxLifes;
    }

    public void DestroyOneLifeUI()
    {
        spriteVitaUI spriteToKill = listaLifes[listaLifes.Count - 1];
        listaLifes.RemoveAt(listaLifes.Count - 1);
        Destroy(spriteToKill.gameObject);
    }
    
}
