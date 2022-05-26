using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Il livello deve essere maggiore di 1 rispetto alle figure")]
    public Livello maxLevel;

    Livello livello = Livello.livello1;


    public void AddLevel()
    {
        livello += 1;
        print(livello);
    }

    public Livello GetLevel()
    {
        return livello;
    }

    public bool GameOver()
    {
        return livello == maxLevel;
    }
}
