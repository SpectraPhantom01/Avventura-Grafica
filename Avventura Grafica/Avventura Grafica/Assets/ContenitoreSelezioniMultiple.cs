using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContenitoreSelezioniMultiple : MonoBehaviour
{
    List<string> listaSelezioni = new List<string>();

    public void AddNewSelection(string name)
    {
        listaSelezioni.Add(name);
    }

    public bool ReadSelection(string name)
    {
        foreach(string a in listaSelezioni)
        {
            if (a.Contains(name))
                return true;
        }
        return false;
    }
}
