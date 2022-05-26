using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSpaceInvaders : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<NemicoSpaceInvaders>() != null)
        {
            SpaceInvadersGameManager.Instance.GameOver();
        }
    }
}
