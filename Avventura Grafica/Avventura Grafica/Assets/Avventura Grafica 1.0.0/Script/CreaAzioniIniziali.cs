using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreaAzioniIniziali : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject oggettoDaSeguire = GameObject.Find("NavCam1");
        if (oggettoDaSeguire.GetComponent<Follower>() != null)
            gameObject.transform.parent = oggettoDaSeguire.gameObject.transform.parent;
    }

    private void Update()
    {
        GameObject oggettoDaSeguire = GameObject.Find("NavCam1");
        Vector3 destination = oggettoDaSeguire.gameObject.transform.position;
        destination.z = 0;
        if (oggettoDaSeguire.GetComponent<Follower>() != null)
            gameObject.transform.position = destination;
    }
}
