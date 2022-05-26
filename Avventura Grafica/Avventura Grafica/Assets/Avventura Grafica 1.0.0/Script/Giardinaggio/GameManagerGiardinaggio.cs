using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManagerGiardinaggio : MonoBehaviour
{
    [Tooltip("Figure da spawnare")]
    [SerializeField] List<GameObject> allFigurePrefab;
    public GameObject cursore;
    public List<int> timers;
    public List<string> messaggiOnMistake;
    public List<string> messaggiOnNextLevel;
    public ScoreManagerGiardinaggio scGiardinaggio;
    public LevelManager livelloGiardinaggio;
    public LineDrawer drawer;
    public LifeGiardinaggio lifeManager;
    public TimerWithEvent timerManager;

    public UnityEvent onEndShape;
    public UnityEvent onNextLevel;
    public UnityEvent onGameOver;

    public TextMeshProUGUI testoCentrale;
    public TextMeshProUGUI testoTimer;
    public GameObject bottoneSi;
    public GameObject bottoneNo;


    [HideInInspector]
    public List<GameObject> figure = new List<GameObject>();

    bool gameStart;
    bool gameOver;
    bool changingLevel;
    bool win;

    [Header("Mistakes")]
    int limitMistakeEdge;
    int mistakeOnEdge = 0;
    [SerializeField] float timeTimedMessage;

    GameObject currentFig;
    GameObject oldFig;

    GameObject centralBanner;

    List<PointGiardinaggio> listaPunti = new List<PointGiardinaggio>();

    Coroutine coroutineTimedMessage;

    public bool GamePaused => !gameStart;

    int limitMistakeEdgeOld;

    int scoreToReach;
    int finalScore;

    bool onFinalMessage;

    internal int GetScoreGoal()
    {
        return scoreToReach;
    }

    // Start is called before the first frame update
    void Awake()
    {
        figure.Clear();
        SpawnAllShapes();
        SetFigure(GetNextFig());
        lifeManager.GetManager(this);

        ActiveButtons(false);
        limitMistakeEdge = lifeManager.GetInitialLifes();
        limitMistakeEdgeOld = limitMistakeEdge;
        centralBanner = testoCentrale.gameObject.transform.parent.gameObject;
    }

    public void ActiveButtons(bool v)
    {
        bottoneSi.gameObject.SetActive(v);
        bottoneNo.gameObject.SetActive(v);
    }

    private void Update()
    {
        UpdateCursor();
        UpdateTimer();
        if (gameStart && !gameOver)
        {
            drawer.CanDraw(true);
        }
        else if (!win)
        {
            PremiPerIniziare();
            drawer.CanDraw(false);
        }

        if (win && !onFinalMessage)
        {
            onFinalMessage = true;
            scGiardinaggio.ScoresOnLevels.ForEach(x => finalScore += x);
            string message = $"Hai vinto e totalizzato {finalScore} punti!";
            SetNewMessage(message, false);
            ActiveButtons(true);
        }
    }

    private void UpdateTimer()
    {
        testoTimer.text = timerManager.GetTimerValue().ToString();
    }

    private void UpdateCursor()
    {
        Cursor.visible = false;
        cursore.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursore.transform.position = new Vector3(cursore.transform.position.x, cursore.transform.position.y, 0);
    }

    private void PremiPerIniziare()
    {
        if (Input.anyKeyDown && !gameOver)
        {
            if (!changingLevel)
            {
                gameStart = true;
                centralBanner.SetActive(false);
                testoCentrale.gameObject.SetActive(false);

            }
            else
            {
                onNextLevel.Invoke();

            }

            if (!win)
                timerManager.PlayTimer();
        }
    }

    private void SpawnAllShapes()
    {
        foreach (GameObject g in allFigurePrefab)
        {
            figure.Add(Instantiate(g));
            g.SetActive(false);
        }
    }

    private void SetFigure(GameObject fig)
    {
        listaPunti.Clear();
        currentFig = fig;

        CheckAllPoints(fig);

        scoreToReach = listaPunti.Count;

        print(scoreToReach);

        scGiardinaggio.ResetScore(scoreToReach);

        currentFig.gameObject.SetActive(true);

    }

    public void EndShape(bool correctLevel)
    {
        if (correctLevel)
            SetNewMessage(messaggiOnNextLevel[(int)livelloGiardinaggio.GetLevel()], false);
        else if (scGiardinaggio.EnoughPercentage())
        {
            SetNewMessage(messaggiOnNextLevel[(int)livelloGiardinaggio.GetLevel()], false);
        }
        else
        {
            SetNewMessage(messaggiOnMistake[(int)livelloGiardinaggio.GetLevel()], false);
        }

        currentFig.GetComponent<Shape>().ClearImage();
        onEndShape.Invoke();
    }

    private void CheckAllPoints(GameObject figura)
    {
        CheckPoint checkpoint = figura.GetComponentInChildren<CheckPoint>();
        foreach (Transform t in checkpoint.gameObject.transform)
        {
            listaPunti.Add(t.gameObject.GetComponent<PointGiardinaggio>());
        }
        checkpoint.GiveManager(this);
    }

    public void NextLevel()
    {

        if (!livelloGiardinaggio.GameOver() && !win)
        {
            oldFig = currentFig;
            livelloGiardinaggio.AddLevel();
            currentFig.gameObject.SetActive(false);
            gameStart = true;
            centralBanner.SetActive(false);
            testoCentrale.gameObject.SetActive(false);
            limitMistakeEdge = limitMistakeEdgeOld;
            ChangeFigure(GetNextFig());
        }
        else
        {
            win = true;
        }
    }

    private GameObject GetNextFig()
    {
        timerManager.SetNewTime(timers[(int)livelloGiardinaggio.GetLevel()]);
        return figure[(int)livelloGiardinaggio.GetLevel()];
    }

    public void ChangeFigure(GameObject newFig)
    {
        newFig.GetComponent<Shape>().ShapeTimer();
        SetFigure(newFig);
    }

    public void ResetCurrentShape()
    {
        CheckPoint check = currentFig.GetComponentInChildren<CheckPoint>();
        foreach (Transform t in check.transform)
        {
            t.GetComponent<PointGiardinaggio>().ResetMe();
        }
    }

    public void SetNewMessage(string message, bool timed)
    {
        centralBanner.SetActive(true);
        testoCentrale.gameObject.SetActive(true);
        testoCentrale.text = message;
        currentFig.GetComponent<Shape>().ChangeSpriteState(true);

        if (timed)
        {
            if (coroutineTimedMessage == null)
                coroutineTimedMessage = StartCoroutine(TimedMessage());
            else
            {
                StopCoroutine(coroutineTimedMessage);
                coroutineTimedMessage = null;
                coroutineTimedMessage = StartCoroutine(TimedMessage());
            }
        }
    }

    IEnumerator TimedMessage()
    {
        yield return new WaitForSeconds(timeTimedMessage);
        centralBanner.SetActive(false);
        testoCentrale.gameObject.SetActive(false);
        currentFig.GetComponent<Shape>().ChangeSpriteState(false);
    }

    public void SetGameStart(bool var)
    {
        gameStart = var;
    }

    public void ChangingLevel(bool var)
    {
        changingLevel = var;
    }

    public void DestroyShapeDone()
    {
        Destroy(oldFig);
    }

    public void Mistake()
    {
        lifeManager.Mistake();
    }

    public void GameOver()
    {
        gameOver = true;
        ActiveButtons(true);
        onGameOver.Invoke();
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToOldScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void AddMistakeOnEdge()
    {
        mistakeOnEdge++;
        SetNewMessage("Stai tagliando male! ti rimangono " + (limitMistakeEdge - mistakeOnEdge) + " tentativi!", true);
        if (mistakeOnEdge > limitMistakeEdge)
        {
            GameOver();
        }
    }

    internal void AddScore()
    {
        scGiardinaggio.AddScore();
    }

}
