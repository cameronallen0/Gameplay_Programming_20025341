using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera instance;
    public CameraManager cameraManager;

    public float sensitivity = 1.0f;
    public float distance = 5.0f;
    public float height = 2.0f;
    public float minDistance = 1.0f;
    public float maxDistance = 10.0f;
    public float minHeight = 0.5f;
    public float maxHeight = 5.0f;
    public float smoothSpeed = 10.0f;

    private Transform target;
    private Vector2 rotation = Vector2.zero;
    private float currentDistance = 0.0f;
    private float currentHeight = 0.0f;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Camera").transform;
        currentDistance = distance;
        currentHeight = height;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        // Get input from the mouse or controller
        Vector2 input = Vector2.zero;
        Gamepad gamepad = Gamepad.current;

        if (gamepad != null && gamepad.rightStick.IsActuated())
        {
            input = gamepad.rightStick.ReadValue();
        }
        else
        {
            input = Mouse.current.delta.ReadValue() * sensitivity;
        }

        input *= sensitivity;
        input.y *= -1;

        // Update the rotation
        rotation += input;
        rotation.y = Mathf.Clamp(rotation.y, -80.0f, 80.0f);

        // Calculate the new position of the camera
        Vector3 position = target.TransformPoint(new Vector3(0, currentHeight, -currentDistance));
        Quaternion rotationQuaternion = Quaternion.Euler(rotation.y, rotation.x, 0.0f);
        position = target.position + rotationQuaternion * new Vector3(0.0f, height, -distance);

        // Check if the camera is colliding with anything
        RaycastHit hit;
        if (Physics.Linecast(target.position, position, out hit))
        {
            currentDistance = Mathf.Clamp(hit.distance, minDistance, maxDistance);
            currentHeight = Mathf.Clamp(hit.point.y - target.position.y, minHeight, maxHeight);
            position = target.position + rotationQuaternion * new Vector3(0.0f, currentHeight, -currentDistance);
        }

        // Update the position and rotation of the camera
        transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * smoothSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotationQuaternion, Time.deltaTime * smoothSpeed);
    }
}