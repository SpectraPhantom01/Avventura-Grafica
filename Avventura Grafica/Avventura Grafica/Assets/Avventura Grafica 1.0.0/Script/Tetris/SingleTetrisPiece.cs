using UnityEngine;

public class SingleTetrisPiece : MonoBehaviour
{
    [SerializeField] ParticleSystem onDestroyVFX;
    TetrisBlock father;
    int id;
    private void Awake()
    {
        father = GetComponentInParent<TetrisBlock>();

    }

    private void Start()
    {
        id = father.Id;
    }

    public bool CheckValidMove(Vector3 nextDirection)
    {
        RaycastHit2D[] hits;

        hits = Physics2D.RaycastAll(transform.position + nextDirection, nextDirection, 0.5f);

        for (int i = 0; i < hits.Length; i++)
        {
            if (!CheckCollisions(hits, i, id)) return false;
        }
        return true;
    }

    public bool CheckCollisions(RaycastHit2D[] hits, int i, int id)
    {
        int hitId;
        if (hits[i])
        {
            TetrisBlock tb = hits[i].collider.GetComponent<TetrisBlock>();
            if (tb != null)
            {
                hitId = tb.Id;
                if (hitId != id)
                {
                    return false;
                }
            }

            Muro muro = hits[i].collider.GetComponent<Muro>();
            if (muro != null)
            {
                return false;
            }
        }
        return true;
    }

    public void Destroy()
    {
        Instantiate(onDestroyVFX, transform.position, Quaternion.identity);
        father.RemovePiece(this);
        Destroy(gameObject);
    }

}
