using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;
    public ButtonController buttons;

    public Camera playerCamera;
    public Camera doorCamera;

    private void Awake()
    {
        ShowPlayerCamera();

        instance = this;
    }

    public void ShowPlayerCamera()
    {
        playerCamera.enabled = true;
        doorCamera.enabled = false;
    }
    public void ShowDoorCamera()
    {
        playerCamera.enabled = false;
        doorCamera.enabled = true;
    }
}
