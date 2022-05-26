using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoorTrigger : MonoBehaviour
{
    //Vecchio script
    public UnityEvent openDoor;
    public UnityEvent closeDoor;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        openDoor.Invoke();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        closeDoor.Invoke();
    }
}
