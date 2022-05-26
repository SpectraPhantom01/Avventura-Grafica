using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TiroAlBersaglioManager : MonoBehaviour
{
    [SerializeField] GameObject cursorSprite;
    // riferimenti ai Prefab
    [SerializeField] GameObject AnatraPrefab;

    // riferimenti alla scena serializzati
    [SerializeField] Camera cameraObj;
    [SerializeField] Canvas canvasObject;
    [SerializeField] Bottone btnSiObject;
    [SerializeField] Bottone btnNoObject;
    [SerializeField] Timer timer;
    [SerializeField] Transform spawnPointTorre1;
    [SerializeField] Transform spawnPointTorre2;
    [SerializeField] Transform spawnPointTorre3;
    [SerializeField] Transform spawnPointTorre4;
    [SerializeField] int livelloLineaSuperiore;
    [SerializeField] int livelloInTorre;
    // variabili serializzate
    [SerializeField] float altezzaLineaDaDx = 2;
    [SerializeField] float altezzaLineaDaSx = -2;
    [SerializeField] int numProiettili = 10;
    [SerializeField] int record;
    [SerializeField] int tempoTimer = 30;

    // variabili locali
    List<Anatra> listaAnatre = new List<Anatra>();
    float posizioneLateraleSpawn;
    int punteggio = 0;
    int resetProiettili;
    bool gameOver = true;
    bool vinto = false;
    bool finito = false;
    bool almenoUnaVittoria = false;

    Camera mainCam;

    // Start is called before the first frame update
    void Start()
    {
        Messaggio("Per iniziare premi un pulsante", "", false);
        resetProiettili = numProiettili;
        posizioneLateraleSpawn = 12;
        record = AC.GlobalVariables.GetIntegerValue(11);
    }

    // Update is called once per frame
    void Update()
    {
        cursorSprite.transform.position = cameraObj.ScreenToWorldPoint(Input.mousePosition);
        cursorSprite.transform.position = new Vector3(cursorSprite.transform.position.x, cursorSprite.transform.position.y, 0);

        if (!gameOver)
        {
            if (numProiettili > 0)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    LanciaRaggio();

                    numProiettili -= 1;
                }

                Messaggio("Punteggio: " + punteggio.ToString() + "/"+ record + "          Proiettili rimasti: " + numProiettili.ToString() , "", false);
            }
            else
            {
                if(punteggio >= record)
                {
                    Messaggio("Hai vinto!", "Vuoi provare a battere il tuo Record di " + punteggio + " ?", true);
                    vinto = true;
                    gameOver = true;
                    almenoUnaVittoria = true;
                    AC.GlobalVariables.SetIntegerValue(11, punteggio);
                }
                else
                {
                    Messaggio("Hai perso!", "Vuoi provare di nuovo?", true);
                    vinto = false;
                }
                FineGioco();
            }
        }
        else
        {
            if(!vinto)
            {
                PremiPerIniziare();
            }
            
        }
    }


    private void FineGioco()
    {
        if (!finito)
        {
            StopAllCoroutines();
            DistruggiTutteAnatre();
            timer.StopTimer(tempoTimer);
            finito = true;
        }
    }

    private void PremiPerIniziare()
    {
        if(Input.anyKeyDown)
        {
            gameOver = false;
            StartCoroutine(GeneraAnatreCoroutine());
            timer.StartTimer(tempoTimer);
        }
    }

    private void Messaggio(string mess, string mess2, bool risposta)
    {
        if (!risposta)
        {
            canvasObject.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = mess;
            MessaggioFinale msg = canvasObject.gameObject.GetComponentInChildren<MessaggioFinale>();
            msg.gameObject.GetComponent<TextMeshProUGUI>().text = mess2;

            btnSiObject.gameObject.SetActive(false);
            btnNoObject.gameObject.SetActive(false);
        }
        else
        {
            canvasObject.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = mess;
            MessaggioFinale msg = canvasObject.gameObject.GetComponentInChildren<MessaggioFinale>();
            msg.gameObject.GetComponent<TextMeshProUGUI>().text = mess2;

            btnSiObject.gameObject.SetActive(true);
            btnNoObject.gameObject.SetActive(true);
        }
    }

    private void LanciaRaggio()
    {
        bool hoColpitoQualcosa;

        RaycastHit2D informazioniSulRaggio = Physics2D.CircleCast(cameraObj.ScreenToWorldPoint(Input.mousePosition), 0.01f, Vector2.up, 0);

        hoColpitoQualcosa = informazioniSulRaggio.collider != null;

        if (hoColpitoQualcosa)
        {
            if (informazioniSulRaggio.collider.gameObject.GetComponent<Anatra>() != null)
            {
                Anatra anatraColpita = informazioniSulRaggio.collider.gameObject.GetComponent<Anatra>();

                punteggio += anatraColpita.GetScore();

                listaAnatre.Remove(anatraColpita);
                anatraColpita.SetColpito();
            }

        }
    }

    private IEnumerator GeneraAnatreCoroutine()
    {

        while (true)
        {
            GameObject a = Instantiate(AnatraPrefab);
            a.gameObject.GetComponent<Anatra>().Inizializza(posizioneLateraleSpawn, altezzaLineaDaDx, Direzione.Sinistra, this);
            a.gameObject.GetComponent<Anatra>().ChangeSpriteOrder(livelloLineaSuperiore);
            listaAnatre.Add(a.GetComponent<Anatra>());

            GameObject b = Instantiate(AnatraPrefab);
            b.gameObject.GetComponent<Anatra>().Inizializza(-posizioneLateraleSpawn, altezzaLineaDaSx, Direzione.Destra, this);
            listaAnatre.Add(b.GetComponent<Anatra>());

            SpawnSpecialAnatra();

            yield return new WaitForSeconds(2f);
        }
    }

    void SpawnSpecialAnatra()
    {
        int rndNum = UnityEngine.Random.Range(1, 8);
        if (rndNum >= 4)
        {
            GameObject c = Instantiate(AnatraPrefab);
            switch(rndNum)
            {
                case 4:
                    c.gameObject.GetComponent<Anatra>().Inizializza(spawnPointTorre1.position, this);
                    break;
                case 5:
                    c.gameObject.GetComponent<Anatra>().Inizializza(spawnPointTorre2.position, this);
                    break;
                case 6:
                    c.gameObject.GetComponent<Anatra>().Inizializza(spawnPointTorre3.position, this);
                    break;
                case 7:
                    c.gameObject.GetComponent<Anatra>().Inizializza(spawnPointTorre4.position, this);
                    break;
            }
            c.gameObject.GetComponent<Anatra>().ChangeSpriteOrder(livelloInTorre);
            listaAnatre.Add(c.GetComponent<Anatra>());
        }
    }    

    public void DistruggiTutteAnatre()
    {
        foreach (Anatra a in listaAnatre)
        {
            a.SetColpito();
        }
    }

    public void AggiornaListaDaAnatra(Anatra anatra)
    {
        listaAnatre.Remove(anatra);
    }

    public void ResettaIlGioco()
    {
        if (vinto)
            record = punteggio;
        numProiettili = resetProiettili;
        punteggio = 0;
        gameOver = true;
        vinto = false;
        finito = false;
        Messaggio("Per iniziare premi un pulsante", "", false);
        listaAnatre = new List<Anatra>();
    }

    public void TimerFinito()
    {
        numProiettili = 0;
    }

    public void CheckRealWin()
    {
        if(almenoUnaVittoria)
        {
            AC.GlobalVariables.SetBooleanValue(9, true);
        }
        else
        {
            AC.GlobalVariables.SetBooleanValue(9, false);
        }

        AC.GlobalVariables.SetBooleanValue(10, true);

        SceneManager.LoadScene("Cap03Bancarelle");
    }
}
