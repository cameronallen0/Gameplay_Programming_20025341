using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public static DoorController instance;

    Animator doorAnimator;

    public GameObject door;

    void Awake()
    {
        doorAnimator = door.GetComponent<Animator>();

        instance = this;
    }
    public void DoorAnim()
    {
        doorAnimator.SetTrigger("Door");
    }
}
