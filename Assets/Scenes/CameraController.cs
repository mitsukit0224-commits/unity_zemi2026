using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("Settings")]
    public float mouseSensitivity = 100f;
    public Transform player; // Inspectorでプレイヤーをアサイン

    [Header("カメラ位置")]
    public float distance = 4f;   // プレイヤーからの距離
    public float height = 1.5f;   // プレイヤーからの高さ

    private float xRotation = 0f;
    private float yRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        var mouse = Mouse.current;
        if (mouse == null) return;

        float mouseX = mouse.delta.x.ReadValue() * mouseSensitivity * Time.deltaTime;
        float mouseY = mouse.delta.y.ReadValue() * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        yRotation += mouseX;

        // カメラの回転を計算
        Quaternion rotation = Quaternion.Euler(xRotation, yRotation, 0f);

        // プレイヤーの「上方向」を重力に合わせて取得
        Vector3 playerUp = -GravityManager.GravityDirection;

        // プレイヤーの位置を基準にカメラを配置
        Vector3 offset = rotation * new Vector3(0f, height, -distance);
        transform.position = player.position + offset;

        // 常にプレイヤーを見る
        transform.LookAt(player.position + playerUp * height * 0.5f);
    }
}