using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AC;

public class Contatore : MonoBehaviour
{
    public int countAmount;
    public GameObject objToChange;
    int counter;
    // Start is called before the first frame update
    void Start()
    {
        counter = 0;
    }

    private void Update()
    {
        if(countAmount == counter)
        {
            objToChange.gameObject.SetActive(true);
        }
        else
        {
            objToChange.gameObject.SetActive(false);
        }
    }

    public void AddOneCounter()
    {
        counter++;
    }

    public void RemoveOneCounter()
    {
        counter--;
    }

    public int GetCount()
    {
        return counter;
    }

    public void AddOrRemoveCounters(int amount)
    {
        counter += amount;
    }

    
}
