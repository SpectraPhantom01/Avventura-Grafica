
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


public class SpaceInvadersGameManager : MonoBehaviour
{
    // variabili STATIC
    static public SpaceInvadersGameManager Instance;
    // riferimenti PREFAB
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject nemico1Prefab;
    [SerializeField] GameObject nemico2Prefab;
    [SerializeField] GameObject nemico3Prefab;
    [SerializeField] GameObject nemico4Prefab;
    [SerializeField] GameObject nemicoSpecialePrefab;
    [SerializeField] GameObject proiettilePlayerPrefab;
    [SerializeField] GameObject proiettileNemicoPrefab;
    [SerializeField] GameObject torreDiDifesaPrefab;
    [SerializeField] GameObject spriteVitaUIPrefab;
    // riferimenti alla SCENA
    [SerializeField] Transform pivotMatriceNemici;
    [SerializeField] Transform pivotTorriDiDifesa;
    [SerializeField] Transform pivotPlayer;
    [SerializeField] Transform pivotNemicoSpeciale;
    [SerializeField] RectTransform pivotViteUI;
    [SerializeField] BaseSpaceInvaders baseplayerObject;
    [SerializeField] Canvas canvasObject;
    [SerializeField] GameObject panel;
    [SerializeField] Camera mainCameraObject;
    // variabili da INSPECTOR
    [SerializeField] int quantitaFile = 4;
    [SerializeField] int quantitaColonne = 12;
    [SerializeField] int numeroVite;
    [SerializeField] int quantitaTests = 15;
    [SerializeField] float tempoMovimentoMatrice = 1;
    [SerializeField] float tempoMaxAspettaSparo = 1;
    [SerializeField] float tempoSpawnNemicoSpeciale = 10;
    // variabili GLOBALI
    public float tempoPassato { get; private set; }
    // variabili LOCALI
    PlayerSpaceInvaders player;
    NemicoSpaceInvaders[][] nemici;
    PunteggioTesto testoObject;
    NemicoSpecialeSpaceInvaders nemicoSpeciale;
    // events
    public UnityEvent OnGameOver;

    Coroutine coroutineSparoMatrice;
    Coroutine coroutineMovimentoMatrice;
    Coroutine coroutineTempoPassato;
    Coroutine coroutineNemicoSpeciale;
    Coroutine coroutineLampeggiaObject;

    List<TorreDifesa> listaTorriDiDifesa;
    List<spriteVitaUI> listaVite;

    Direzione direzioneMatrice;
    Livello livello;

    bool iniziato;
    bool vittoria;
    int score = 0;
    int nemiciUccisi;
    bool gamePaused;

    // ---------------------------------------------------------- START ---------------------------------------------
    // ---------------------------------------------------------- START ---------------------------------------------
    // ---------------------------------------------------------- START ---------------------------------------------

    void Awake()
    {
        Instance = this;
        if (quantitaFile > 4)
        {
            quantitaFile = 4;
        }
        livello = Livello.livello1;
        vittoria = false;
        tempoPassato = 0;
        nemiciUccisi = 0;
        InizializzaMatrice();
        listaTorriDiDifesa = new List<TorreDifesa>();
        listaVite = new List<spriteVitaUI>();
        iniziato = false;
        direzioneMatrice = Direzione.Destra;
        testoObject = canvasObject.gameObject.GetComponentInChildren<PunteggioTesto>();
        testoObject.gameObject.SetActive(false);
        coroutineLampeggiaObject = StartCoroutine(LampeggiaObject(canvasObject.gameObject.GetComponentInChildren<MessaggioFinale>().gameObject));

        gamePaused = false;

    }


    // ---------------------------------------------------------- UPDATE ---------------------------------------------
    // ---------------------------------------------------------- UPDATE ---------------------------------------------
    // ---------------------------------------------------------- UPDATE ---------------------------------------------
    void Update()
    {
        if(!iniziato && !vittoria && !gamePaused)
        {
            PremiPerIniziare();
        }
        if (iniziato)
        {
            AggiornaTesto();
            ControlloStatoNemici();
        }
    }

    private void ControlloStatoNemici()
    {
        int totalitaNemici = quantitaFile * quantitaColonne;
        if (totalitaNemici - nemiciUccisi <= 10)
        {
            livello = Livello.livello2;
        }
        if (totalitaNemici - nemiciUccisi <= 4)
        {
            livello = Livello.livello3;
        }

        if (nemiciUccisi == totalitaNemici)
        {
            vittoria = true;
            GameOver();
        }
    }

    private void PremiPerIniziare()
    {
        if (Input.anyKeyDown)
        {
            iniziato = true;
            IniziaGioco();
        }
    }

    private void AggiornaTesto()
    {
        testoObject.gameObject.GetComponent<TextMeshProUGUI>().text = "SCORE:  " + score.ToString();
    }

    // ---------------------------------------------------------- LOGICA NEMICO ---------------------------------------------
    // ---------------------------------------------------------- LOGICA NEMICO ---------------------------------------------
    // ---------------------------------------------------------- LOGICA NEMICO ---------------------------------------------

    private void SelezionaNemicoCheSpara(int quantitaNemici)
    {
        int numeroNemiciCheSparano;
        if(quantitaNemici < 5)
        {
            numeroNemiciCheSparano = 1;
        }
        else if (quantitaNemici < 9)
        {
            numeroNemiciCheSparano = 2;
        }
        else
        {
            numeroNemiciCheSparano = 3;
        }
        for (int i = 0; i < numeroNemiciCheSparano; i++)
        {
            NemicoSpaceInvaders n = GetNemicoCheSpara();
            if(n != null)
                n.AttivamiPerSparare();
        }
    }

    private NemicoSpaceInvaders GetNemicoCheSpara()
    {
        
        for (int y = quantitaFile - 1; y >= 0; y--)
        {
            for (int x = 0; x < quantitaTests; x++)
            {
                int rndNum = UnityEngine.Random.Range(0, quantitaColonne);

                for (int i = 0; i < y; i++)
                {
                    if(nemici[y - i][rndNum] != null)
                    {
                        return nemici[y - i][rndNum];
                    }
                }
            }
        }
        return null;
    }

    private void SpostamentoMatrice(int xOffset, int yOffset)
    {
        pivotMatriceNemici.position = new Vector2(pivotMatriceNemici.position.x + xOffset, pivotMatriceNemici.position.y + yOffset);
    }

    private NemicoSpaceInvaders GetNewNemico(int index)
    {
        switch(index)
        {
            case 0:
                return Instantiate(nemico1Prefab.gameObject.GetComponent<NemicoSpaceInvaders>(), pivotMatriceNemici);
            case 1:
                return Instantiate(nemico2Prefab.gameObject.GetComponent<NemicoSpaceInvaders>(), pivotMatriceNemici);
            case 2:
                return Instantiate(nemico3Prefab.gameObject.GetComponent<NemicoSpaceInvaders>(), pivotMatriceNemici);
            case 3:
                return Instantiate(nemico4Prefab.gameObject.GetComponent<NemicoSpaceInvaders>(), pivotMatriceNemici);
            default:
                return null;
        }
    }


    //  ----------------------------------------------------------------------------------ISTANZE INIZIALI----------------------------------------------------
    //  ----------------------------------------------------------------------------------ISTANZE INIZIALI----------------------------------------------------
    //  ----------------------------------------------------------------------------------ISTANZE INIZIALI----------------------------------------------------
    private void InizializzaMatrice()
    {
        nemici = new NemicoSpaceInvaders[quantitaFile][];
        for (int y = 0; y < nemici.Length; y++)
        {
            nemici[y] = new NemicoSpaceInvaders[quantitaColonne];
        }
    }
    private void IniziaGioco()
    {
        StopCoroutine(coroutineLampeggiaObject);
        coroutineTempoPassato = StartCoroutine(TimeElapsed());
        testoObject.gameObject.SetActive(true);
        MessaggioFinale messaggioCentrale = canvasObject.gameObject.GetComponentInChildren<MessaggioFinale>();
        if (messaggioCentrale != null)
            messaggioCentrale.gameObject.SetActive(false);
        panel.SetActive(false);
        GeneraNemici();
        GeneraNemicoSpeciale();
        GeneraTorri();
        GeneraVite();
        GeneraPlayer();

        IniziaMovimentoMatrice();
        IniziaASparare();
    }

    private void GeneraNemicoSpeciale()
    {
        nemicoSpeciale = Instantiate(nemicoSpecialePrefab.GetComponent<NemicoSpecialeSpaceInvaders>(), pivotNemicoSpeciale);
        nemicoSpeciale.InizializzaNemicoSpeciale(pivotNemicoSpeciale, proiettileNemicoPrefab);
        coroutineNemicoSpeciale = StartCoroutine(AttivaNemicoSpeciale(tempoSpawnNemicoSpeciale));
    }



    private void GeneraVite()
    {
        for (int i = 0; i < numeroVite; i++)
        {
            listaVite.Add(Instantiate(spriteVitaUIPrefab.gameObject.GetComponent<spriteVitaUI>(), pivotViteUI));
            listaVite[i].InizializzaVita(i, pivotViteUI);
        }
    }

    private void GeneraPlayer()
    {
        player = Instantiate(playerPrefab.gameObject.GetComponent<PlayerSpaceInvaders>(), pivotPlayer);
        player.InizializzaGiocatore(pivotPlayer, mainCameraObject, proiettilePlayerPrefab, listaVite);
    }

    private void GeneraTorri()
    {
        listaTorriDiDifesa.Clear();
        for (int i = 0; i < 3; i++)
        {
            listaTorriDiDifesa.Add(Instantiate(torreDiDifesaPrefab.gameObject.GetComponent<TorreDifesa>(), pivotTorriDiDifesa));
            listaTorriDiDifesa[i].InizializzaTorre(i, pivotTorriDiDifesa);
        }

    }

    private void GeneraNemici()
    {
        int i = 4;
        for (int y = 0; y < nemici.Length; y++)
        {
            for (int x = 0; x < nemici[0].Length; x++)
            {
                nemici[y][x] = GetNewNemico(y);
                nemici[y][x].InizializzaNemico(y, i, x, pivotMatriceNemici, proiettileNemicoPrefab);
            }
            i--;
        }
    }

    //  ----------------------------------------------------------------------------------COROUTINES----------------------------------------------------
    //  ----------------------------------------------------------------------------------COROUTINES----------------------------------------------------
    //  ----------------------------------------------------------------------------------COROUTINES----------------------------------------------------
    private void IniziaASparare()
    {
        coroutineSparoMatrice = StartCoroutine(Spara());
    }

    private void IniziaMovimentoMatrice()
    {
        coroutineMovimentoMatrice = StartCoroutine(MovimentoMatrice());
    }

    private IEnumerator AttivaNemicoSpeciale(float tempo)
    {
        yield return new WaitForSeconds(tempo);
        nemicoSpeciale.Attivami();
    }

    private IEnumerator TimeElapsed()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            tempoPassato += 1;
        }
    }
    private IEnumerator MovimentoMatrice()
    {
        int counter = 0;
        Direzione tempDir = Direzione.none;
        while (livello == Livello.livello1)
        {
            yield return new WaitForSeconds(tempoMovimentoMatrice);
            MuoviMatriceNemici(ref counter, ref tempDir);
        }
        while (livello == Livello.livello2)
        {
            yield return new WaitForSeconds(tempoMovimentoMatrice - 0.5f);
            MuoviMatriceNemici(ref counter, ref tempDir);
        }
        while(livello == Livello.livello3)
        {
            yield return new WaitForSeconds(tempoMovimentoMatrice - 0.8f);
            MuoviMatriceNemici(ref counter, ref tempDir);
        }
    }

    private void MuoviMatriceNemici(ref int counter, ref Direzione tempDir)
    {
        switch (direzioneMatrice)
        {
            case Direzione.Destra:
                SpostamentoMatrice(1, 0);
                counter++;
                break;
            case Direzione.Sinistra:
                SpostamentoMatrice(-1, 0);
                counter++;
                break;
            case Direzione.Giu:
                SpostamentoMatrice(0, -1);
                if (tempDir == Direzione.Destra)
                    direzioneMatrice = Direzione.Sinistra;
                else
                    direzioneMatrice = Direzione.Destra;
                break;
            default:
                break;
        }
        if (counter >= 10)
        {
            counter = 0;
            tempDir = direzioneMatrice;
            direzioneMatrice = Direzione.Giu;
        }
    }

    private IEnumerator Spara()
    {
        while (true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, tempoMaxAspettaSparo));
            SelezionaNemicoCheSpara(UnityEngine.Random.Range(1, 11));
        }
    }
    private IEnumerator LampeggiaObject(GameObject oggettoDaLampeggiare)
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            oggettoDaLampeggiare.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            oggettoDaLampeggiare.SetActive(true);
        }
    }

    // ---------------------------------------------------------------------- LOGICA PUNTEGGIO E GAME OVER ----------------------
    // ---------------------------------------------------------------------- LOGICA PUNTEGGIO E GAME OVER ----------------------
    // ---------------------------------------------------------------------- LOGICA PUNTEGGIO E GAME OVER ----------------------

    public void AddScore(int quantita)
    {
        score += quantita;
        if (quantita < 1000)
            nemiciUccisi++;
        else
        {
            StopCoroutine(coroutineNemicoSpeciale);
            coroutineNemicoSpeciale = StartCoroutine(AttivaNemicoSpeciale(tempoSpawnNemicoSpeciale));
        }
    }

    internal void GameOver()
    {
        StopAllCoroutines();
        panel.SetActive(true);
        if(vittoria)
        {

            Cursor.visible = true;
            iniziato = false;
            nemicoSpeciale.Disattivami();
            player.Bloccami();
            MessaggioCentrale("HAI VINTO!!!");
            OnGameOver.Invoke();

        }
        else
        {

            Cursor.visible = true;
            iniziato = false;
            nemicoSpeciale.Disattivami();
            player.Bloccami();
            MessaggioCentrale("HAI PERSO!!!");
            OnGameOver.Invoke();

        }
        gamePaused = true;
    }

    private void MessaggioCentrale(string messaggio)
    {
        for (int i = 0; i < canvasObject.transform.childCount; i++)
        {
            canvasObject.transform.GetChild(i).gameObject.SetActive(true);
        }
        canvasObject.gameObject.GetComponentInChildren<MessaggioFinale>().gameObject.GetComponent<TextMeshProUGUI>().text = messaggio;
    }

    public void ExitGame()
    {
        SceneManager.LoadScene(1);
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(4);
    }
}
