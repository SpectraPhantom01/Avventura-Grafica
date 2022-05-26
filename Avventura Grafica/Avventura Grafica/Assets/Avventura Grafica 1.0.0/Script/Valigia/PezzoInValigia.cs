using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PezzoInValigia : MonoBehaviour
{
    [SerializeField] BoxCollider2D superiorCollider;
    [SerializeField] BoxCollider2D inferiorCollider;
    public bool necessario;
    [SerializeField] UnityEvent onPickUpFirstTime;
    
    float castRadius;

    Rigidbody2D rBody;

    bool pickedUp;
    bool availableToPickUp = true;

    bool imInside = false;

    Camera mainCam;

    public DroppingArea dropArea { get; private set; }

    GameManagerMGValigia gmValigia;

    bool inInventory = false;

    // Start is called before the first frame update
    void Start()
    {
        GameManagerMGValigia.Instance.Register(this);

        rBody = GetComponent<Rigidbody2D>();

        rBody.gravityScale = 0.5f;

        mainCam = Camera.main;

        castRadius = GetComponent<BoxCollider2D>().size.x;
    }

    private void Update()
    {
        if (pickedUp)
        {
            float xLine = mainCam.ScreenToWorldPoint(Input.mousePosition).x;

            if(xLine < dropArea.leftEdge.position.x)
            {
                xLine = dropArea.leftEdge.position.x;
            }
            else if(xLine > dropArea.rightEdge.position.x)
            {
                xLine = dropArea.rightEdge.position.x;
            }

            gameObject.transform.position = new Vector3( xLine, gameObject.transform.position.y, 0);

            superiorCollider.enabled = false;
            
        }
    }

    public void PutMeInInventory(Vector3 upLeft, Vector3 lowRight)
    {
        inInventory = true;

        float rndX = UnityEngine.Random.Range(upLeft.x, lowRight.x);
        float rndY = UnityEngine.Random.Range(upLeft.y, lowRight.y);

        transform.position = new Vector3(rndX, rndY);

        onPickUpFirstTime.Invoke();
    }

    private bool CanBePickedUp()
    {
        // raycast

        castRadius = GetComponent<BoxCollider2D>().size.x;

        RaycastHit2D[] infos = Physics2D.BoxCastAll(superiorCollider.gameObject.transform.position, new Vector2(castRadius, 0.5f * castRadius) * transform.localScale, 0, Vector2.zero);
        
        availableToPickUp = true;

        for (int i = 0; i < infos.Length; i++)
        {
            PezzoInValigia p = infos[i].collider.gameObject.GetComponent<PezzoInValigia>();
            if (p != null && p != this)
            {
                availableToPickUp = false;
            }
        }


        if (inInventory)
        {
            if (!imInside)
                return true;
            return availableToPickUp;
        }
        else
            return false;
    }

    private void OnDrawGizmos()
    {
        castRadius = GetComponent<BoxCollider2D>().size.x;
        Gizmos.color = Color.red;
        Gizmos.DrawCube(superiorCollider.gameObject.transform.position, new Vector2(castRadius, 0.5f * castRadius) * transform.localScale);
        
    }

    internal void AvailableToPickUp(bool available)
    {
        availableToPickUp = available;
    }

    private void Picking()
    {
        pickedUp = true;
        rBody.isKinematic = true;
        rBody.velocity = Vector3.zero;
    }

    public void FinishPicking()
    {
        pickedUp = false;
        rBody.isKinematic = false;
        superiorCollider.enabled = true;
    }


    public bool TryToGetMe()
    {
        if (CanBePickedUp())
        {
            Picking();
            return true;
        }
        return false;
    }

    internal void GoToDroppingPosition(Vector3 droppingPosition, DroppingArea drop, GameManagerMGValigia gm)
    {
        gmValigia = gm;
        dropArea = drop;
        if (!imInside)
            transform.position = droppingPosition;
        else
            transform.position = new Vector3(transform.position.x, droppingPosition.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        LineaDiFondo muro = collision.collider.gameObject.GetComponent<LineaDiFondo>();
        if (muro != null)
        {
            if (pickedUp)
            {
                pickedUp = false;
            }
        }

    }


    internal void ImInside(bool inside)
    {
        imInside = inside;
    }


    internal void GoBackToDroppingPosition()
    {
        imInside = false;
        transform.position = dropArea.pivotDropping.position;
        Picking();
        gmValigia.BackOnPicking(this);
    }


}
