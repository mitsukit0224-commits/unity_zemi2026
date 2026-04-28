using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("Settings")]
    public float mouseSensitivity = 100f;
    public Transform player; // Inspectorでプレイヤーをアサイン

    private float xRotation = 0f;

    void Start()
    {
        // マウスカーソルを非表示＆ロック
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        var mouse = Mouse.current;
        if (mouse == null) return;

        float mouseX = mouse.delta.x.ReadValue() * mouseSensitivity * Time.deltaTime;
        float mouseY = mouse.delta.y.ReadValue() * mouseSensitivity * Time.deltaTime;

        // 上下の視点（カメラのみ動く）
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f); // 見上げ・見下ろしの制限

        // カメラの上下回転はローカルで適用
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // 左右の回転はプレイヤーごと回転
        player.Rotate(Vector3.up * mouseX);
    }
}