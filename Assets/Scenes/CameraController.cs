using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("マウス設定")]
    public float mouseSensitivity = 100f;
    public Transform player;

    [Header("カメラ距離")]
    public float distance = 4f;
    public float minDistance = 1f;
    public float maxDistance = 10f;
    public float zoomSpeed = 2f;
    public float height = 1.5f;

    private float xRotation = 0f;
    private float yRotation = 0f;

    // PlayerMovementから参照する用
    public Vector3 CameraForward { get; private set; }
    public Vector3 CameraRight   { get; private set; }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        yRotation = 0f;
    }

    void LateUpdate()
    {
        HandleZoom();
        HandleRotation();
    }

    void HandleZoom()
    {
        var mouse = Mouse.current;
        if (mouse == null) return;

        float scroll = mouse.scroll.y.ReadValue();
        distance -= scroll * zoomSpeed * Time.deltaTime * 10f;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);
    }

    void HandleRotation()
    {
        var mouse = Mouse.current;
        if (mouse == null) return;

        float mouseX = mouse.delta.x.ReadValue() * mouseSensitivity * Time.deltaTime;
        float mouseY = mouse.delta.y.ReadValue() * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        yRotation += mouseX;

        Vector3 gravityUp = -GravityManager.GravityDirection;

        // yRotationだけで水平方向の回転を作る
        Quaternion yRot = Quaternion.AngleAxis(yRotation, gravityUp);
        Quaternion xRot = Quaternion.AngleAxis(xRotation, yRot * Vector3.right);
        Quaternion finalRotation = xRot * yRot;

        // カメラ位置を計算
        Vector3 offset = finalRotation * new Vector3(0f, 0f, -distance);
        transform.position = player.position + gravityUp * height + offset;
        transform.LookAt(player.position + gravityUp * height * 0.5f, gravityUp);

        // 移動用の前方向・右方向を保存（上下成分は除去）
        CameraForward = Vector3.ProjectOnPlane(yRot * Vector3.forward, GravityManager.GravityDirection).normalized;
        CameraRight   = Vector3.ProjectOnPlane(yRot * Vector3.right,   GravityManager.GravityDirection).normalized;
    }
}