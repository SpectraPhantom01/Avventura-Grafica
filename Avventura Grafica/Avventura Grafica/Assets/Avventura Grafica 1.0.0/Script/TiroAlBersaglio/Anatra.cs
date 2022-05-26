using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direzione { Destra, Sinistra, Su, Giu, none}

public class Anatra : MonoBehaviour
{
    // variabili globali
    [SerializeField] float speedAnatra;
    [SerializeField] float speedBonusAnatra;
    [SerializeField] float speedSuperBonusAnatra;
    [SerializeField] float alezzaMax;
    [SerializeField] float altezzaMin;
    [SerializeField] float scaleBiggerPoint;
    [SerializeField] float timerDeactivate;
    [SerializeField] float timerDeactivateFastAnatra;
    

    // variabili locali
    bool bonus;
    bool superBonus;
    int punteggio;
    TiroAlBersaglioManager manager;
    Vector2 velocity;
    Direzione direzioneTenuta = Direzione.none;
    Rigidbody2D rd;
    Coroutine coroutineTimer;
    Vector3 startPosition;
    private void Start()
    {
        rd = gameObject.GetComponent<Rigidbody2D>();
        velocity = new Vector2(speedAnatra, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (superBonus)
        {
            UpAndDown();
        }
        else
        {
            Vector2 movimento = Vector2.zero;

            switch (direzioneTenuta)
            {
                case Direzione.Destra:
                    movimento = rd.position + velocity * Time.fixedDeltaTime;

                    break;
                case Direzione.Sinistra:

                    movimento = rd.position - velocity * Time.fixedDeltaTime;
                    break;
                case Direzione.none:
                    break;
            }

            rd.MovePosition(movimento);

            if (gameObject.transform.position.x > 14f || gameObject.transform.position.x < -14f)
            {
                DestroyWithoutScore();
            }
        }
    }

    private void DestroyWithoutScore()
    {
        manager.AggiornaListaDaAnatra(this);
        Destroy(gameObject);
    }

    private void UpAndDown()
    {
        
        gameObject.transform.position += new Vector3(0, speedSuperBonusAnatra) * Time.fixedDeltaTime;
        if(Vector3.Distance(startPosition, gameObject.transform.position) > alezzaMax && direzioneTenuta == Direzione.Su)
        {
            speedSuperBonusAnatra = -speedSuperBonusAnatra;
            direzioneTenuta = Direzione.Giu;
            return;
        }
        if(Vector3.Distance(startPosition, gameObject.transform.position) > altezzaMin && direzioneTenuta == Direzione.Giu)
        {
            DestroyWithoutScore();
        }
    }

    public void SetColpito()
    {
        Destroy(gameObject);
    }

    public int GetScore()
    {
        return punteggio;
    }

    public void Inizializza(float x, float y, Direzione dir, TiroAlBersaglioManager _manager)
    {
        gameObject.transform.position = new Vector3(x, y);
        manager = _manager;

        if (Random.Range(1, 7) >= 5)
        {
            bonus = true;
        }

        if (!bonus)
            punteggio = Random.Range(1, 3);
        else
        {
            punteggio = 5;
            timerDeactivate = timerDeactivateFastAnatra;
            gameObject.transform.localScale = new Vector3(scaleBiggerPoint, scaleBiggerPoint, scaleBiggerPoint);
            speedAnatra = speedBonusAnatra;
        }

        direzioneTenuta = dir;

        if(speedAnatra <= 0)
        {
            speedAnatra = 1;
        }

        if(direzioneTenuta == Direzione.Destra)
        {
            gameObject.GetComponentInChildren<Transform>().Rotate(new Vector3(0, 180, 0));
        }
    }

    public void Inizializza(Vector3 spawnPosition, TiroAlBersaglioManager _manager)
    {
        gameObject.transform.position = spawnPosition;
        manager = _manager;

        superBonus = true;

        punteggio = 10;
        
        gameObject.transform.localScale = new Vector3(0.45f, 0.45f, 0.45f);

        startPosition = spawnPosition;
        direzioneTenuta = Direzione.Su;
    }

    public void ChangeSpriteOrder(int newLevel)
    {
        gameObject.GetComponentInChildren<SpriteRenderer>().sortingOrder = newLevel;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<TorreCentraleTiroBersaglio>() != null)
        {
            DeactivateMe();
        }
    }

    void DeactivateMe()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        if(coroutineTimer == null)
            coroutineTimer = StartCoroutine(StartTimer());
    }

    IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(timerDeactivate);
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        coroutineTimer = null;
    }
}
