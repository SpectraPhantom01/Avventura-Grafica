using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NemicoSpaceInvaders : MonoBehaviour
{

    [SerializeField] Transform pivotSparoObj;
    [SerializeField] float potenzaSparo;
    [SerializeField] GameObject particleVFXOnDestroy;
    [SerializeField] GameObject splashVFXOnDestroy;
    [SerializeField] Color myColor;

    SpaceInvadersGameManager spGm;
    GameObject proiettileNemicoObject;
    int valoreNemico;
    bool possoSparare;


    private void Update()
    {
        if(possoSparare)
        {
            Spara();
        }
    }

    private void Start()
    {
        spGm = SpaceInvadersGameManager.Instance;
    }

    private void Spara()
    {
        ProiettileSpaceInvaders proiettileSparato = Instantiate(proiettileNemicoObject).gameObject.GetComponent<ProiettileSpaceInvaders>();
        proiettileSparato.InizializzaProiettile(false, pivotSparoObj, potenzaSparo, gameObject, false, 0);
        possoSparare = false;
    }

    public void InizializzaNemico(int y, int i, int x, Transform pivot, GameObject bullet)
    {
        gameObject.transform.position = new Vector2(pivot.position.x + x * 2.3f , pivot.position.y - y * 2.3f);
        valoreNemico = 50 * i;
        proiettileNemicoObject = bullet;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ProiettileSpaceInvaders bulletPlayer = collision.gameObject.GetComponent<ProiettileSpaceInvaders>();
        if (bulletPlayer != null)
        {
            if (bulletPlayer.isPlayerShooting)
            {
                spGm.AddScore(valoreNemico);

                ParticleSystem pc = Instantiate(particleVFXOnDestroy, transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
                Instantiate(splashVFXOnDestroy, transform.position, Quaternion.identity);
                var main = pc.main;
                main.startColor = myColor;

                Destroy(gameObject);
            }
        }
    }



    public void AttivamiPerSparare()
    {
        possoSparare = true;
    }
}
