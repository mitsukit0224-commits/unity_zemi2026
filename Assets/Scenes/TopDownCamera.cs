using UnityEngine;
using UnityEngine.InputSystem;

public class TopDownCamera : MonoBehaviour
{
    [Header("── マウス感度 ──")]
    public float mouseSensitivity = 5f;

    [Header("── ズーム ──")]
    public float minDistance    = 2f;
    public float maxDistance    = 15f;
    public float zoomSpeed      = 5f;
    public float zoomSmoothTime = 0.1f;

    [Header("── 追従対象 ──")]
    public Transform player;

    [Header("── カメラ位置 ──")]
    public float distance = 8f;
    public float height   = 1.5f;

    private float xRotation       = 20f;
    private float yRotation       = 0f;
    private float currentDistance;
    private float distanceVelocity = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible   = true;
        currentDistance  = distance;
    }

    void LateUpdate()
    {
        if (player == null) return;

        var mouse = Mouse.current;
        if (mouse == null) return;

        if (mouse.leftButton.isPressed)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible   = false;

            float mouseX = mouse.delta.x.ReadValue() * mouseSensitivity * Time.deltaTime;
            float mouseY = mouse.delta.y.ReadValue() * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation  = Mathf.Clamp(xRotation, -80f, 80f);
            yRotation += mouseX;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible   = true;
        }

        float scroll = mouse.scroll.y.ReadValue();
        distance -= scroll * zoomSpeed;
        distance  = Mathf.Clamp(distance, minDistance, maxDistance);
        currentDistance = Mathf.SmoothDamp(
            currentDistance, distance,
            ref distanceVelocity, zoomSmoothTime);

        Vector3 gravityUp = -GravityManager.GravityDirection.normalized;

        Quaternion gravityFrame = Quaternion.FromToRotation(Vector3.up, gravityUp);
        Quaternion localRot     = Quaternion.Euler(xRotation, yRotation, 0f);
        Quaternion worldRot     = gravityFrame * localRot;

        Vector3 offset = worldRot * new Vector3(0f, height, -currentDistance);

        transform.position = player.position + offset;
        transform.LookAt(player.position + gravityUp * height * 0.5f, gravityUp);
    }
}