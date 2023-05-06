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
    public DoorCamera doorCam;

    public GameObject button;
    Animator wallbuttonAnimator;

    public bool canPress;
    public bool cameraMove = false;
    private bool timerGoing = false;
    private bool buttonHit = false;
    private bool doorOpen = false;

    private void Awake()
    {
        instance = this;

        wallbuttonAnimator = button.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!doorOpen)
        {
            canPress = true;
        } 
    }
    private void OnTriggerExit(Collider other)
    {
        if(!doorOpen)
        {
            canPress = false;
        }
    }
    public void WallButtonAnim()
    {
        if(!doorOpen)
        {
            StartCoroutine(Timer());
            wallbuttonAnimator.SetTrigger("WallButton");
            cam.ShowDoorCamera();
            player.OnDisable();
        }
    }
    public void MoveDoor()
    {
        if(buttonHit)
        {
            door.DoorAnim();
            doorOpen = true;
        }
    }
    IEnumerator Timer()
    {
        timerGoing = true;
        buttonHit = true;

        yield return new WaitForSeconds(1.5f);

        cameraMove = true;
        
        MoveDoor();
    
        yield return new WaitForSeconds(4f);

        timerGoing = false;

        if (!timerGoing)
        {
            cam.ShowPlayerCamera();
            player.OnEnable();
        }
    }
}
