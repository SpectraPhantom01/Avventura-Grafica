using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Tetrade { T, L, J, O, I, S, Z}

public class TetrisManager : MonoBehaviour
{
    [SerializeField] Transform _spawnPoint;
    [SerializeField] List<TetrisBlock> prefabTetrisBlocks;
    [Header("Inserire numero di linee da distruggere per il next level")]
    [Tooltip("Lo 0 conta come il primo livello da superare. Il contatore si azzera ad ogni livello")]
    [SerializeField] List<int> levelSteps;
    [Tooltip("Suggerimento: al livello 0 impostare il valore su 0.8, diminuire dai successivi")]
    [SerializeField] List<float> levelSpeed;
    [SerializeField] Transform queueFirstPlacePivot;
    [SerializeField] Transform queueSecondPlacePivot;
    [Header("Scegliere il livello di diffocltà e il punteggio minimo per vincere")]
    [SerializeField] int winnerLevel;
    [SerializeField] int winnerScore = 10000;
    [Header("Colore casse OnGameOver")]
    [SerializeField] Color gameOverBoxColor;
    [Header("UI Manager")]
    [SerializeField] UITetrisManager UITetrisManager;

    Queue<TetrisBlock> _nextBlocks = new Queue<TetrisBlock>();

    float previousTime;
    public static int height { get; private set; } = 20;
    public static int width { get; private set; } = 10;
    public static SingleTetrisPiece[,] grid = new SingleTetrisPiece[width, height];

    private static TetrisManager _instance;
    public static TetrisManager Instance => _instance;

    TetrisBlock currentBlock;
    int nextFreeNumber;
    float gameTime = 0;
    bool gameStarted = false;
    int _difficolta = 1;
    int stepsAchieved;
    float fallTime = 0.8f;

    int currentScore;
    int combos;

    bool doneTetris;

    public int Difficolta // Difficoltà = Livello
    {
        get => _difficolta;
        set
        {
            _difficolta = Mathf.Clamp(value, 1, levelSpeed.Count);
            SetNewFallTime(_difficolta);
            NotifyNewDifficulty(_difficolta);
        }
    }

    private void SetNewFallTime(int value)
    {
        if (value - 1 >= levelSpeed.Count) return;

        fallTime = levelSpeed[value - 1];
    }

    private void NotifyNewDifficulty(int value)
    {
        UITetrisManager.NewDifficultyText(value.ToString());
    }

    void Awake()
    {
        _instance = this;
        winnerLevel = Mathf.Clamp(winnerLevel, 1, levelSteps.Count);
        
    }

    public void StartGame()
    {
        Pool();
        PieceBlocked();
        UpdateStepText();
        gameStarted = true;
    }

    void Update()
    {
        if (gameStarted)
            gameTime += Time.deltaTime;

        if (currentBlock != null)
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                currentBlock.Move(Direzione.Sinistra);
            }
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                currentBlock.Move(Direzione.Destra);
            }
            if(Input.GetKeyDown(KeyCode.Q))
            {
                currentBlock.Rotate(90);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                currentBlock.Rotate(-90);
            }
            if(Input.GetKeyDown(KeyCode.F))
            {
                SwitchQueue();
            }

            if (gameTime - previousTime > ((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) ? fallTime / 10 : fallTime))
            {
                currentBlock.Move(Direzione.Giu);
                previousTime = gameTime;
            }
        }
    }

    public void PieceBlocked()
    {
        CheckLines();
        
        currentBlock = DequeueFromPool();
        currentBlock.transform.position = _spawnPoint.position;
        currentBlock.transform.position += new Vector3(currentBlock.SpawnXOffset, currentBlock.SpawnYOffset);
        Pool();
    }

    private TetrisBlock DequeueFromPool()
    {
        TetrisBlock temp1 = _nextBlocks.Dequeue();
        TetrisBlock temp2 = _nextBlocks.Dequeue();
        temp2.transform.position = queueFirstPlacePivot.position;
        _nextBlocks.Enqueue(temp2);

        return temp1;

    }

    private void SwitchQueue()
    {
        TetrisBlock temp1 = _nextBlocks.Dequeue();
        TetrisBlock temp2 = _nextBlocks.Dequeue();
        temp2.transform.position = queueFirstPlacePivot.position;
        temp1.transform.position = queueSecondPlacePivot.position;

        _nextBlocks.Enqueue(temp2);
        _nextBlocks.Enqueue(temp1);
    }

    public void GameOver()
    {
        currentBlock = null;

        StartCoroutine(OnGameOverEffect());
    }

    private IEnumerator OnGameOverEffect()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (grid[x, y] != null)
                {
                    grid[x, y].GetComponent<SpriteRenderer>().color = gameOverBoxColor;
                    yield return new WaitForEndOfFrame();
                }
            }
        }


        if (Difficolta > winnerLevel && currentScore > winnerScore)
        {
            UITetrisManager.OnEnd(true, Difficolta, currentScore);
        }
        else
        {
            UITetrisManager.OnEnd(false, Difficolta, currentScore);
        }
    }

    void Pool()
    {
        if (_nextBlocks.Count == 0)
            _nextBlocks.Enqueue(GetNewPiece(queueFirstPlacePivot));

        _nextBlocks.Enqueue(GetNewPiece(queueSecondPlacePivot));
    }

    private TetrisBlock GetNewPiece(Transform spawnPoint)
    {
        int rndNum = UnityEngine.Random.Range(0, 7);
        TetrisBlock newTb = Instantiate(prefabTetrisBlocks[rndNum], spawnPoint.position, Quaternion.identity);

        nextFreeNumber++;
        newTb.Initialize(nextFreeNumber);

        return newTb;
    }

    void CheckLines()
    {
        for (int i = height - 1; i >= 0; i--)
        {
            if(HasLine(i))
            {
                DeleteLines(i);
                RowDown(i);
                AddLevel();
            }
        }
        UpdateScore();
    }

    private void UpdateScore()
    {
        if(doneTetris)
        {
            if (combos > 0)
            {
                currentScore += 1200 * _difficolta;
                combos = 0;
                UITetrisManager.UpdateScore(currentScore.ToString());
            }
            else
            {
                doneTetris = false;
                print("Bonus not active");
            }
        }

        if (combos == 0) return;

        if (combos == 1)
            currentScore += 100 * _difficolta;
        else if (combos == 2)
            currentScore += 250 * _difficolta;
        else if (combos == 3)
            currentScore += 500 * _difficolta;
        else
        {
            currentScore += 800 * _difficolta;
            doneTetris = true;
            print("Bonus active");
        }

        UITetrisManager.UpdateScore(currentScore.ToString());
        combos = 0;
    }

    private void AddLevel()
    {
        stepsAchieved++;
        if (levelSteps[_difficolta - 1] <= stepsAchieved)
        {
            Difficolta++;
            stepsAchieved = 0;
        }
        combos++;
        UpdateStepText();
        
    }

    private void UpdateStepText()
    {
        UITetrisManager.UpdateStepText((levelSteps[_difficolta - 1] - stepsAchieved).ToString());
    }

    private void RowDown(int i)
    {
        for (int y = i; y < height; y++)
        {
            for (int j = 0; j < width; j++)
            {
                if(grid[j,y] != null)
                {
                    grid[j, y - 1] = grid[j, y];
                    grid[j, y] = null;
                    grid[j, y - 1].transform.position -= new Vector3(0, 1, 0);
                }
            }
        }
    }

    private void DeleteLines(int i)
    {
        for (int j = 0; j < width; j++)
        {
            
            grid[j, i].Destroy();
            grid[j, i] = null;
        }
    }

    private bool HasLine(int i)
    {
        for (int j = 0; j < width; j++)
        {
            if (grid[j, i] == null) return false;
        }

        return true;
    }

    public void AddDifficolta(int value)
    {
        Difficolta += value;
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextScene()
    {
        SceneManager.LoadScene(0);
    }
}
