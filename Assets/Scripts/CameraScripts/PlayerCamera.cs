using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera instance;
    public CameraManager cameraManager;
    public PlayerController player;

    public Transform target;
    public float distance = 5.0f;
    public float height = 3.0f;
    public float smoothSpeed = 1f;
    public float rotationSpeed = 1.5f;

    private Vector3 offset;
    private float currentRotationX;
    private float currentRotationY;

    private InputAction rotateCameraActionX;
    private InputAction rotateCameraActionY;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        offset = transform.position - target.position;
        currentRotationX = 0.0f;
        currentRotationY = 0.0f;

        rotateCameraActionX = new InputAction("RotateCameraX", InputActionType.Value, "<Mouse>/delta/x");
        rotateCameraActionX.Enable();

        rotateCameraActionY = new InputAction("RotateCameraY", InputActionType.Value, "<Mouse>/delta/y");
        rotateCameraActionY.Enable();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void FixedUpdate()
    {
        float horizontal = rotateCameraActionX.ReadValue<float>();
        currentRotationX += horizontal * rotationSpeed;

        float vertical = rotateCameraActionY.ReadValue<float>();
        currentRotationY += vertical * rotationSpeed;

        Vector3 desiredPosition = target.position + offset;
        desiredPosition += Quaternion.Euler(currentRotationY, currentRotationX, 0) * Vector3.back * distance;
        desiredPosition += Vector3.up * height;

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        transform.position = smoothedPosition;

        transform.LookAt(target);
    }
    void OnDestroy()
    {
        rotateCameraActionX.Disable();
        rotateCameraActionX.Dispose();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
