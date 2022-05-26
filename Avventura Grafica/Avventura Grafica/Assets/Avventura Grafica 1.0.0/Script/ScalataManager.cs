using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ScalataManager : MonoBehaviour
{
    static public ScalataManager Instance;

    // grafica tasti
    [SerializeField] Tasto tastoA;
    [SerializeField] Tasto tastoS;
    [SerializeField] Tasto tastoD;
    [SerializeField] Tasto tastoF;

    // punteggi prefab
    [SerializeField] GameObject APointPrefab;
    [SerializeField] GameObject SPointPrefab;
    [SerializeField] GameObject DPointPrefab;
    [SerializeField] GameObject FPointPrefab;

    // spawn punteggi
    [SerializeField] GameObject ASpawnPointPrefab;
    [SerializeField] GameObject SSpawnPointPrefab;
    [SerializeField] GameObject DSpawnPointPrefab;
    [SerializeField] GameObject FSpawnPointPrefab;

    // RIFERIMENTI ALLA SCENA hit point prefab
    [SerializeField] GameObject hitAPrefab;
    [SerializeField] GameObject hitSPrefab;
    [SerializeField] GameObject hitDPrefab;
    [SerializeField] GameObject hitFPrefab;
    [SerializeField] GameObject lineaDiFondoPrefab;
    [SerializeField] ScoreManager scoreManager;

    // Canvas
    [SerializeField] GameObject btnSi;
    [SerializeField] GameObject btnNo;
    [SerializeField] Canvas canvasObject;

    // variabile da inspector
    [SerializeField] float startSecondiSpawnLettere = 2f;
    [SerializeField] float velocizzatoreSecondiSpawn = 0f;
    [SerializeField] float velocitaDiscesa = 100f;

    // eventi
    [Header("Pressing button on keyboard")]
    [SerializeField] UnityEvent onPressButtonA;
    [SerializeField] UnityEvent onPressButtonS;
    [SerializeField] UnityEvent onPressButtonD;
    [SerializeField] UnityEvent onPressButtonF;
    [Header("Releasing button on keyboard")]
    [SerializeField] UnityEvent onReleaseButtonA;
    [SerializeField] UnityEvent onReleaseButtonS;
    [SerializeField] UnityEvent onReleaseButtonD;
    [SerializeField] UnityEvent onReleaseButtonF;

    // variabili locali
    HitPoint hitA;
    HitPoint hitS;
    HitPoint hitD;
    HitPoint hitF;

    SpawnPoint spawnA;
    SpawnPoint spawnS;
    SpawnPoint spawnD;
    SpawnPoint spawnF;

    LetteraManager APoint;

    LetteraManager SPoint;
    LetteraManager DPoint;
    LetteraManager FPoint;

    int score = 0;
    int quantitaBlocchiPersi = 0;
    bool gameOver = false;
    bool iniziato = false;
    int quantitaGotAnything = 0;

    Coroutine coroutineSpawnBlocchi;
    Coroutine coroutineMessaggioErrore;
    List<LetteraManager> listaBlocchi;
    TextMeshProUGUI testoLivello;
    TextMeshProUGUI testoScore;
    TextMeshProUGUI messaggioCentrale;
    // Start is called before the first frame update
    void Awake()
    {

        Instance = this;

        hitA = hitAPrefab.GetComponent<HitPoint>();
        hitS = hitSPrefab.GetComponent<HitPoint>();
        hitD = hitDPrefab.GetComponent<HitPoint>();
        hitF = hitFPrefab.GetComponent<HitPoint>();

        spawnA = ASpawnPointPrefab.GetComponent<SpawnPoint>();
        spawnS = SSpawnPointPrefab.GetComponent<SpawnPoint>();
        spawnD = DSpawnPointPrefab.GetComponent<SpawnPoint>();
        spawnF = FSpawnPointPrefab.GetComponent<SpawnPoint>();

        APoint = APointPrefab.GetComponent<LetteraManager>();
        SPoint = SPointPrefab.GetComponent<LetteraManager>();
        DPoint = DPointPrefab.GetComponent<LetteraManager>();
        FPoint = FPointPrefab.GetComponent<LetteraManager>();

        spawnA.gameObject.transform.position = new Vector3(hitA.gameObject.transform.position.x, spawnA.gameObject.transform.position.y);
        spawnS.gameObject.transform.position = new Vector3(hitS.gameObject.transform.position.x, spawnS.gameObject.transform.position.y);
        spawnD.gameObject.transform.position = new Vector3(hitD.gameObject.transform.position.x, spawnD.gameObject.transform.position.y);
        spawnF.gameObject.transform.position = new Vector3(hitF.gameObject.transform.position.x, spawnF.gameObject.transform.position.y);

        testoScore = canvasObject.gameObject.GetComponentInChildren<PunteggioTesto>().gameObject.GetComponent<TextMeshProUGUI>();
        testoLivello = canvasObject.gameObject.GetComponentInChildren<LivelloTesto>().gameObject.GetComponent<TextMeshProUGUI>();
        messaggioCentrale = canvasObject.gameObject.GetComponentInChildren<MessaggioFinale>().gameObject.GetComponent<TextMeshProUGUI>();
        
        AssegnazioniIniziali();

    }

    private void AssegnazioniIniziali()
    {
        listaBlocchi = new List<LetteraManager>();

        btnSi.gameObject.SetActive(false);
        btnNo.gameObject.SetActive(false);

        score = 0;
        quantitaBlocchiPersi = 0;

        AggiornaTesto("", testoScore);
        AggiornaTesto("", testoLivello);
        AggiornaTesto("Premi un tasto per iniziare", messaggioCentrale);
        hitA.gameObject.SetActive(false);
        hitS.gameObject.SetActive(false);
        hitD.gameObject.SetActive(false);
        hitF.gameObject.SetActive(false);

        testoScore.gameObject.SetActive(false);
        testoLivello.gameObject.SetActive(false);
        gameOver = false;
        iniziato = false;
        coroutineSpawnBlocchi = null;
        velocizzatoreSecondiSpawn = 0f;
        quantitaGotAnything = 0;

        //tastoA.SwitchMeOff();
        //tastoS.SwitchMeOff();
        //tastoD.SwitchMeOff();
        //tastoF.SwitchMeOff();
    }

    public void PremiPerIniziare()
    {
        if(Input.anyKeyDown)
        {
            iniziato = true;
            testoScore.gameObject.SetActive(true);
            testoLivello.gameObject.SetActive(true);
            messaggioCentrale.gameObject.SetActive(false);
            hitA.gameObject.SetActive(true);
            hitS.gameObject.SetActive(true);
            hitD.gameObject.SetActive(true);
            hitF.gameObject.SetActive(true);
            coroutineSpawnBlocchi = StartCoroutine(GeneraLettere());
        }
    }

    private IEnumerator GeneraLettere()
    {
        int rndNum;
        while (true)
        {
            rndNum = UnityEngine.Random.Range(0, 4);
            switch (rndNum)
            {
                case 0:
                    listaBlocchi.Add(Instantiate(APoint, ASpawnPointPrefab.transform.localPosition, Quaternion.identity));
                    break;
                case 1:
                    listaBlocchi.Add(Instantiate(SPoint, SSpawnPointPrefab.transform.localPosition, Quaternion.identity));
                    break;
                case 2:
                    listaBlocchi.Add(Instantiate(DPoint, DSpawnPointPrefab.transform.localPosition, Quaternion.identity));
                    break;
                case 3:
                    listaBlocchi.Add(Instantiate(FPoint, FSpawnPointPrefab.transform.localPosition, Quaternion.identity));
                    break;
            }

            yield return new WaitForSeconds(startSecondiSpawnLettere + velocizzatoreSecondiSpawn);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!iniziato)
        {
            PremiPerIniziare();
        }
        if (!gameOver && iniziato)
        {
            InputGetKeyDown();
            InputGetKeyUp();
        }
    }

    private void InputGetKeyDown()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (!hitA.Click())
            {
                GotAnything();
            }
            OnHitAEvent();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (!hitS.Click())
            {
                GotAnything();
            }
            OnHitSEvent();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            if(!hitD.Click())
            {
                GotAnything();
            }
            OnHitDEvent();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if(!hitF.Click())
            {
                GotAnything();
            }
            OnHitFEvent();
        }
    }

    private void GotAnything()
    {
        quantitaGotAnything++;
        if(quantitaGotAnything == 5)
        {
            coroutineMessaggioErrore = StartCoroutine(MessaggioErroreATempo("Attento! Non stai colpendo nulla!"));
        }
        if(quantitaGotAnything == 10)
        {
            StopCoroutine(coroutineMessaggioErrore);
            coroutineMessaggioErrore = StartCoroutine(MessaggioErroreATempo("Se continui perderai punti"));
        }
        if(quantitaGotAnything >= 15)
        {
            AddScore(-1);
        }
    }

    private IEnumerator MessaggioErroreATempo(string messaggio)
    {
        messaggioCentrale.gameObject.SetActive(true);
        AggiornaTesto(messaggio, messaggioCentrale);
        yield return new WaitForSeconds(3);
        messaggioCentrale.gameObject.SetActive(false);
    }

    private void InputGetKeyUp()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            OnReleaseAEvent();
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            OnReleaseSEvent();
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            OnReleaseDEvent();
        }
        if (Input.GetKeyUp(KeyCode.F))
        {
            OnReleaseFEvent();
        }
    }

    public void OnHitAEvent()
    {
        onPressButtonA.Invoke();
    }
    public void OnHitSEvent()
    {
        onPressButtonS.Invoke();
    }
    public void OnHitDEvent()
    {
        onPressButtonD.Invoke();
    }
    public void OnHitFEvent()
    {
        onPressButtonF.Invoke();
    }

    public void OnReleaseAEvent()
    {
        onReleaseButtonA.Invoke();
    }
    public void OnReleaseSEvent()
    {
        onReleaseButtonS.Invoke();
    }
    public void OnReleaseDEvent()
    {
        onReleaseButtonD.Invoke();
    }
    public void OnReleaseFEvent()
    {
        onReleaseButtonF.Invoke();
    }

    private void AggiornaTesto(string messaggio, TextMeshProUGUI meshMessaggio)
    {
        meshMessaggio.text = messaggio;
    }

    public void AddScore(int amount)
    {
        score += amount;
    }
    public int GetScore()
    {
        return score;
    }
    public void DiminuisciLista(LetteraManager letteraDaTogliere)
    {
        listaBlocchi.Remove(letteraDaTogliere);
    }
    public float VelocitaAggiornata()
    {
        return velocitaDiscesa;
    }

    public void AumentaVelocitaDiscesa(float amount)
    {
        velocitaDiscesa += amount;
    }

    public void DecrementaSecondiSpawn(float amount)
    {
        velocizzatoreSecondiSpawn -= amount;
    }

    public void GiocoFinito()
    {
        if (!gameOver)
        {
            StopCoroutine(coroutineSpawnBlocchi);
            StopCoroutine(coroutineMessaggioErrore);
            foreach (LetteraManager l in listaBlocchi)
            {
                l.DistruggimiSenzaPunteggio();
            }
            gameOver = true;
            messaggioCentrale.gameObject.SetActive(true);
            AggiornaTesto("Vuoi giocare ancora?", messaggioCentrale);
            btnSi.SetActive(true);
            btnNo.SetActive(true);
        }
    }

    public void AggiornaTestoLivelloOScore(string nuovoTesto, bool livello)
    {
        if(livello)
        {
            AggiornaTesto(nuovoTesto, testoLivello);
        }
        else
        {
            AggiornaTesto(nuovoTesto, testoScore);
        }
    }

    public int GetBlocchiPersi()
    {
        return quantitaBlocchiPersi;
    }

    public void AddBloccoPerso(int quantita)
    {
        quantitaBlocchiPersi += quantita;
    }

    public void ResetGame()
    {
        AssegnazioniIniziali();
        scoreManager.ResetScore();
    }

}
