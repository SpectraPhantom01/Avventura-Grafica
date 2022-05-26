using System;
using System.Collections.Generic;
using UnityEngine;


public class TetrisBlock : MonoBehaviour
{
    [SerializeField] Transform rotationPoint;
    [SerializeField] Tetrade myType;
    [SerializeField] List<SingleTetrisPiece> myPieces;
    [SerializeField] GameObject rotationChecker;
    [SerializeField] float spawnXOffset;
    [SerializeField] float spawnYOffset;

    private int _id;
    public int Id => _id;
    public float SpawnXOffset => spawnXOffset;
    public float SpawnYOffset => spawnYOffset;

    public Tetrade TetradeType => myType;

    private Vector3 previousPosition;
    int lastDirection;


    public void Initialize(int newId)
    {
        _id = newId;
    }

    public void Move(Direzione direction)
    {
        previousPosition = transform.position;
        switch (direction)
        {
            case Direzione.Destra:

                NextMove(new Vector3(1, 0, 0));

                break;

            case Direzione.Sinistra:

                NextMove(new Vector3(-1, 0, 0));

                break;

            case Direzione.Giu:

                NextMove(new Vector3(0, -1, 0));

                if (previousPosition == transform.position)
                {

                    Destroy(rotationChecker);

                    if (AddToGrid())
                        TetrisManager.Instance.PieceBlocked();
                }

                break;

            case Direzione.none:
                break;
        }
    }

    public bool AddToGrid()
    {
        foreach (SingleTetrisPiece pieces in myPieces)
        {
            float roundedX = (pieces.transform.position.x - 0.5f);
            float roundedY = (pieces.transform.position.y - 0.5f);
            int x = System.Convert.ToInt32(roundedX);
            int y = System.Convert.ToInt32(roundedY);

            if (x >= TetrisManager.width || y >= TetrisManager.height)
            {
                TetrisManager.Instance.GameOver();
                return false;
            }

            TetrisManager.grid[x, y] = pieces;
        }

        return true;
    }

    public void Rotate(int direction)
    {
        direction = SpecificCase(direction);

        rotationChecker.transform.RotateAround(rotationPoint.position, new Vector3(0, 0, 1), direction);
        if (ValidMove())
        {
            transform.RotateAround(rotationPoint.position, new Vector3(0, 0, 1), direction);
            rotationChecker.transform.RotateAround(rotationPoint.position, new Vector3(0, 0, 1), direction * -1);
            lastDirection = direction;
        }
        else
            rotationChecker.transform.RotateAround(rotationPoint.position, new Vector3(0, 0, 1), direction * -1);

    }

    private int SpecificCase(int direction)
    {
        switch (myType)
        {
            case Tetrade.T:
                break;
            case Tetrade.L:
                break;
            case Tetrade.J:
                break;
            case Tetrade.O:
                break;
            case Tetrade.I:
                if (lastDirection < 0)
                    direction = Mathf.Abs(direction);
                else if (direction > 0)
                    direction *= -1;
                break;
            case Tetrade.S:
                break;
            case Tetrade.Z:
                break;
        }

        return direction;
    }

    private bool ValidMove()
    {
        foreach (Transform children in rotationChecker.transform)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(children.position, children.position + Vector3.right, 1f);

            for (int i = 0; i < hits.Length; i++)
            {
                if (!CheckCollisions(hits, i, _id)) return false;
            }
        }
        return true;
    }


    private void NextMove(Vector3 direction)
    {

        foreach (SingleTetrisPiece tPiece in myPieces)
        {
            if (!tPiece.CheckValidMove(direction))
            {
                return;
            }
        }

        transform.position += direction;

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

    public void RemovePiece(SingleTetrisPiece singleTetrisPiece)
    {
        myPieces.Remove(singleTetrisPiece);
        if (myPieces.Count == 0)
            Destroy(gameObject);
    }
}
