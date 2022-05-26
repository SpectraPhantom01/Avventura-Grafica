using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LetteraManager : MonoBehaviour
{
    bool distruttibile;
    Rigidbody2D rd;
    ScalataManager scM;
    // Start is called before the first frame update
    private void Awake()
    {
        distruttibile = false;
    }
    void Start()
    {
        rd = gameObject.GetComponent<Rigidbody2D>();
        scM = ScalataManager.Instance;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rd.velocity = Vector2.down * Time.fixedDeltaTime * scM.VelocitaAggiornata();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HitPoint hitp = collision.gameObject.GetComponent<HitPoint>();
        if (hitp != null)
        {
            SetDistruttibile(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        HitPoint hitp = collision.gameObject.GetComponent<HitPoint>();
        if (hitp != null)
        {
            SetDistruttibile(false);
        }
    }

    private void SetDistruttibile(bool v)
    {
        distruttibile = v;
    }

    public void Distruggimi()
    {
        if(distruttibile)
        {
            scM.AddScore(1);
            scM.DiminuisciLista(this);
            Destroy(gameObject);
        }
    }

    public void DistruggimiSenzaPunteggio()
    {
            Destroy(gameObject);
    }

    public void BloccoPerso()
    {
        scM.AddScore(-1);
        scM.AddBloccoPerso(1);
        scM.DiminuisciLista(this);
        Destroy(gameObject);
    }
}
