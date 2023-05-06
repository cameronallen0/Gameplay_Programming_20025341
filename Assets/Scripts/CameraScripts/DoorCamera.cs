using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCamera : MonoBehaviour
{
    public static DoorCamera instance;
    public ButtonController button;
    public Transform newPos;
    public Transform lookPos;
    public float speed = 1f;

    private Vector3 startPos;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        startPos = transform.position;
        transform.LookAt(lookPos);
    }
    private void FixedUpdate()
    {
        if(button.cameraMove)
        {
            transform.LookAt(lookPos);
            float t = speed * Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, newPos.position, t);
        }
    }
}
