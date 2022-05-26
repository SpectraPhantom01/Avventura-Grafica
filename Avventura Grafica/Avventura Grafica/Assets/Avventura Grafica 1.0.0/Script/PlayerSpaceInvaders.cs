using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpaceInvaders : MonoBehaviour
{

    [SerializeField] Transform pivotSparoObj;
    [SerializeField] float tempoAspettaSparo = 1f;
    [SerializeField] float speed;
    [SerializeField] float rotationSpeed;
    [SerializeField] GameObject VFXSpawnOnDeath;
    [SerializeField] GameObject VFXParticleSpawnOnDeath;
    [SerializeField] Vector3 spawnOffset;
    [SerializeField] SpriteRenderer spriteRenderer;



    Camera cameraObj;
    GameObject proiettileObject;
    List<spriteVitaUI> listaVite;
    ProiettileSpaceInvaders proiettileSparato;
    bool possoSparare;
    bool bloccato = false;
    Transform pivotReset;
    public float potenzaSparo = 100f;
    Rigidbody2D rbody;
    BoxCollider2D myCollider;
    private void Awake()
    {
        possoSparare = true;
        rbody = gameObject.GetComponent<Rigidbody2D>();
        myCollider = GetComponent<BoxCollider2D>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        // movimento
        if (!bloccato)
        {
            if (Input.GetAxis("Mouse X") != 0)
            {
                float horizontal;

                horizontal = cameraObj.ScreenToWorldPoint(Input.mousePosition).x;

                Vector2 direction = new Vector2(horizontal, gameObject.transform.position.y);

                rbody.MovePosition(Vector2.Lerp(gameObject.transform.position, direction, speed * Time.fixedDeltaTime));
            }
        }
    }

    private void Update()
    {
        // spara
        if (!bloccato)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (possoSparare)
                {

                    Spara();

                    StartCoroutine(TempoDiAttesaPerSparare());  // <<<<---- per ora questa funziona senza l'altro comando derivante dal proiettile (metodo AbilitamiAlloSparo del player)
                }
            }
        }
    }

    private IEnumerator TempoDiAttesaPerSparare()
    {
        yield return new WaitForSeconds(tempoAspettaSparo);
        if(!possoSparare)
            possoSparare = true;
    }

    private void Spara()
    {
        proiettileSparato = Instantiate(proiettileObject).gameObject.GetComponent<ProiettileSpaceInvaders>();
        proiettileSparato.InizializzaProiettile(true, pivotSparoObj, potenzaSparo, gameObject, true, rotationSpeed);
        possoSparare = false;
    }

    internal void AbilitamiAlloSparo()
    {
        //possoSparare = true;
    }

    public void InizializzaGiocatore(Transform pivot, Camera cam, GameObject bullet, List<spriteVitaUI> vite)
    {
        pivotReset = pivot;
        gameObject.transform.position = pivotReset.position;
        cameraObj = cam;
        proiettileObject = bullet;
        listaVite = vite;
        bloccato = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ProiettileSpaceInvaders bulletCollision = collision.gameObject.GetComponent<ProiettileSpaceInvaders>();
        NemicoSpaceInvaders enemyCollision = collision.gameObject.GetComponent<NemicoSpaceInvaders>();
        if (bulletCollision != null)
        {
            DistruggiPlayer();
        }
        if (enemyCollision != null)
        {
            DistruggiPlayer();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.gameObject.GetComponent<NemicoSpaceInvaders>() != null)
        {
            DistruggiPlayer();
        }
    }

    private void DistruggiPlayer()
    {
        Instantiate(VFXSpawnOnDeath, transform.position, Quaternion.identity);
        Instantiate(VFXParticleSpawnOnDeath, transform.position + spawnOffset, Quaternion.identity);
        StartCoroutine(DestroyAndRespawn());
    }

    IEnumerator DestroyAndRespawn()
    {
        Bloccami();
        spriteRenderer.enabled = false;
        myCollider.enabled = false;
        if (listaVite.Count > 0)
        {
            yield return new WaitForSeconds(2);
            myCollider.enabled = true;
            gameObject.transform.position = pivotReset.position;
            listaVite[listaVite.Count - 1].GetVita();
            listaVite.RemoveAt(listaVite.Count - 1);
            bloccato = false;
            spriteRenderer.enabled = true;
        }
        else
        {
            SpaceInvadersGameManager.Instance.GameOver();
            Destroy(gameObject);
        }
    }

    internal void Bloccami()
    {
        bloccato = true;
    }
}
