using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProiettileSpaceInvaders : MonoBehaviour
{
    public bool isPlayerShooting { get; private set; }
    GameObject personaggioCheHaSparato;
    bool rotateOnSHoot;
    float speedRotationOnShoot;
    // Start is called before the first frame update
    void Start()
    {
        if (isPlayerShooting)
            Invoke("Distruggimi", 2);
        else
            Invoke("Distruggimi", 5);
    }

    private void Update()
    {
        if(rotateOnSHoot)
        {
            gameObject.transform.Rotate(-Vector3.forward * speedRotationOnShoot * Time.deltaTime);
        }
    }

    public void InizializzaProiettile(bool isPlayer, Transform pivot, float potenzaSparo, GameObject personaggioCheSpara, bool rotation, float speedRotation)
    {
        isPlayerShooting = isPlayer;
        personaggioCheHaSparato = personaggioCheSpara;
        gameObject.transform.position = pivot.position;
        rotateOnSHoot = rotation;
        speedRotationOnShoot = speedRotation;
        if (!isPlayerShooting)
        {
            gameObject.layer = 12;
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.down * (Time.deltaTime + potenzaSparo);
        }
        else
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.up * (Time.deltaTime + potenzaSparo);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!isPlayerShooting)
        {
            if(collision.gameObject.GetComponent<NemicoSpaceInvaders>() != null)
            {
                return;
            }
        }
        if(collision.gameObject.GetComponent<ProiettileSpaceInvaders>() != null)
        {
            return;
        }
        if(collision.gameObject.GetComponent<BaseSpaceInvaders>() != null)
        {
            return;
        }
        Distruggimi();
    }

    private void Distruggimi()
    {
        if (isPlayerShooting)
            personaggioCheHaSparato.GetComponent<PlayerSpaceInvaders>().AbilitamiAlloSparo();
        Destroy(gameObject);
    }
}
