using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoorScript : MonoBehaviour
{
    Animator doorAnimator;
    Collider2D col;

    private void Start()
    {
        doorAnimator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
    }

    public void EnableCollider()
    {
        col.enabled = true;
    }

    public void DisableCollider()
    {
        col.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        doorAnimator.Play("Open");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        doorAnimator.Play("Closed");
    }
}
