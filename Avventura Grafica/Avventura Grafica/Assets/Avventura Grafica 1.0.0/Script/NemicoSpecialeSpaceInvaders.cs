using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NemicoSpecialeSpaceInvaders : MonoBehaviour
{
    [SerializeField] float velocita = 10;
    [SerializeField] Transform pivotSparoObj;
    [SerializeField] float potenzaSparo = 10;
    [SerializeField] int valorePunteggio = 1000;
    [SerializeField] GameObject punteggioDaSpawnarePrefab;
    [SerializeField] GameObject vfxSpawnPrefab;

    PunteggioDaSpawnare punteggio;
    SpaceInvadersGameManager spGM;
    Transform pivotNemicoSpeciale;
    GameObject proiettile;
    Coroutine coroutineSparo;
    public bool attivato { get; private set; } = false;
    bool colpito;
    bool stoSparando;
    Direzione dir;


    // Update is called once per frame
    void FixedUpdate()
    {
        if (attivato)
        {
            if (dir == Direzione.Destra)
            {
                gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.right * Time.fixedDeltaTime * velocita;
                if(gameObject.transform.position.x > Mathf.Abs(pivotNemicoSpeciale.position.x))
                {
                    Disattivami();
                }
            }
            else
            {
                gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.left * Time.fixedDeltaTime * velocita;
                if (gameObject.transform.position.x < pivotNemicoSpeciale.position.x)
                {
                    Disattivami();
                }
            }
        }
    }

    private void Update()
    {
        if(colpito)
        {
            attivato = false;
        }
    }

    public void InizializzaNemicoSpeciale(Transform pivot, GameObject bullet)
    {
        pivotNemicoSpeciale = pivot;
        proiettile = bullet;
        attivato = false;
        stoSparando = false;
        colpito = false;
        dir = Direzione.Destra;
        spGM = SpaceInvadersGameManager.Instance;
    }

    public void Attivami()
    {
        attivato = true;
        if (!stoSparando)
        {
            coroutineSparo = StartCoroutine(Sparo());
        }
    }

    private IEnumerator Sparo()
    {
        stoSparando = true;
        while(true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(3, 10));
            ProiettileSpaceInvaders proiettileSparato = Instantiate(proiettile).gameObject.GetComponent<ProiettileSpaceInvaders>();
            proiettileSparato.InizializzaProiettile(false, pivotSparoObj, potenzaSparo, gameObject, false, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<ProiettileSpaceInvaders>() != null)
        {
            Disattivami();
            spGM.AddScore(valorePunteggio);
        }
    }

    public void Disattivami()
    {
        attivato = false;
        colpito = true;
        stoSparando = false;

        Instantiate(punteggioDaSpawnarePrefab,transform.position, Quaternion.identity);
        Instantiate(vfxSpawnPrefab, transform.position, Quaternion.identity);

        gameObject.transform.position = ScegliPivotRandom();
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        StopCoroutine(coroutineSparo);
    }

    private Vector3 ScegliPivotRandom()
    {
        if(UnityEngine.Random.Range(1, 3) == 1)
        {
            dir = Direzione.Destra;
            return pivotNemicoSpeciale.position;
        }
        else
        {
            dir = Direzione.Sinistra;
            float xOpposta = Mathf.Abs(pivotNemicoSpeciale.position.x);
            return new Vector3(xOpposta, pivotNemicoSpeciale.position.y);
        }
    }
}
