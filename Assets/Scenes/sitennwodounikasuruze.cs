using UnityEngine;
using UnityEngine.InputSystem;

public class sitennwodounikasuruze : MonoBehaviour
{
    [Header("Settings")]
    public float mouseSensitivity = 5f;

    [Header("ズーム")]
    public float minDistance = 2f;
    public float maxDistance = 15f;
    public float zoomSpeed = 5f;
    public float zoomSmoothTime = 0.1f;

    public Transform player;

    [Header("カメラ位置")]
    public float distance = 4f;
    public float height = 1.5f;

    private float xRotation = 0f;
    private float yRotation = 0f;
    private float currentDistance;
    private float distanceVelocity = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        currentDistance = distance;
    }

    void LateUpdate()
    {
        var mouse = Mouse.current;
        if (mouse == null) return;

        if (mouse.leftButton.isPressed)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            float mouseX = mouse.delta.x.ReadValue() * mouseSensitivity * Time.deltaTime;
            float mouseY = mouse.delta.y.ReadValue() * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -80f, 80f);
            yRotation += mouseX;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        float scroll = mouse.scroll.y.ReadValue();
        distance -= scroll * zoomSpeed;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);

        currentDistance = Mathf.SmoothDamp(currentDistance, distance, ref distanceVelocity, zoomSmoothTime);

        Quaternion rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        Vector3 playerUp = -GravityManager.GravityDirection;
        Vector3 offset = rotation * new Vector3(0f, height, -currentDistance);
        transform.position = player.position + offset;
        transform.LookAt(player.position + playerUp * height * 0.5f);
    }
}