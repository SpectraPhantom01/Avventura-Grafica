using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ObjectPool : MonoBehaviour
{
    [SerializeField] Transform destination;
    [SerializeField] float force;
    [SerializeField] float stoppingForce;
    [SerializeField] GameManagerAllegroChirurgo gm;
    bool firstBurst;
    Rigidbody2D rbody;
    Vector2 direction;
    Vector3 fixedDestination;
    BoxCollider2D bCollider;



    private void Awake()
    {
        fixedDestination = destination.position;
    }
    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        rbody.freezeRotation = true;
        rbody.gravityScale = 0f;
        bCollider = GetComponent<BoxCollider2D>();
        bCollider.enabled = false;
        direction = fixedDestination - transform.position;
        direction.Normalize();
        firstBurst = true;

        gm.AddOneToPezziTotali(this);
    }

    private void FixedUpdate()
    {
        if (firstBurst)
        {
            if (Vector2.Distance(transform.position, fixedDestination) > 5f)
                rbody.velocity = direction * Time.fixedDeltaTime * force;
            else
            {
                rbody.velocity -= direction * Time.fixedDeltaTime * stoppingForce;
                if (Vector2.Distance(transform.position, fixedDestination) < 0.1f)
                {
                    rbody.velocity = Vector2.zero;
                    firstBurst = false;
                    
                    gm.PezzoPronto();
                }

            }
        }
    }

    public void EnadleCollider()
    {
        bCollider.enabled = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, destination.position);
        Gizmos.DrawSphere(destination.position, 0.5f);
    }
}
