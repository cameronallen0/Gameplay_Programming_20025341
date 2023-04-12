using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public static ButtonController instance;
    public CameraManager cam;
    public PlayerController player;
    public DoorController door;

    public GameObject button;
    Animator buttonAnimator;

    public bool canPress;
    private bool timerGoing = false;
    private bool buttonHit = false;
    private bool doorOpen = false;

    private void Awake()
    {
        instance = this;

        buttonAnimator = button.GetComponentInChildren<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(!doorOpen)
        {
            canPress = true;
            Debug.Log("Can Press");
        } 
    }
    private void OnTriggerExit(Collider other)
    {
        if(!doorOpen)
        {
            canPress = false;
            Debug.Log("Can't Press");
        }
    }
    public void ButtonAnim()
    {
        if(!doorOpen)
        {
            StartCoroutine(Timer());
            buttonAnimator.SetTrigger("Button");
            cam.ShowDoorCamera();
            player.OnDisable();
            Debug.Log("BUTTON");
        }
    }
    public void MoveDoor()
    {
        if(buttonHit)
        {
            door.DoorAnim();
            Debug.Log("DOOR");
            doorOpen = true;
        }
    }
    IEnumerator Timer()
    {
        timerGoing = true;
        buttonHit = true;

        yield return new WaitForSeconds(2.5f);

        MoveDoor();

        yield return new WaitForSeconds(3.5f);

        timerGoing = false;

        if (!timerGoing)
        {
            cam.ShowPlayerCamera();
            player.OnEnable();
        }
    }
}
