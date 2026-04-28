using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void HandleMovement()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        float horizontal = 0f;
        float vertical = 0f;

        if (keyboard.aKey.isPressed) horizontal = -1f;
        if (keyboard.dKey.isPressed) horizontal = 1f;
        if (keyboard.wKey.isPressed) vertical = 1f;
        if (keyboard.sKey.isPressed) vertical = -1f;

        // 現在の重力方向を取得
        Vector3 gravityDir = GravityManager.GravityDirection;

        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight   = Camera.main.transform.right;

        // 重力方向の成分を除去
        camForward = (camForward - gravityDir * Vector3.Dot(camForward, gravityDir)).normalized;
        camRight   = (camRight   - gravityDir * Vector3.Dot(camRight,   gravityDir)).normalized;

        Vector3 moveVelocity = (camRight * horizontal + camForward * vertical) * moveSpeed;

        // 重力方向の速度は維持
        float gravitySpeed = Vector3.Dot(rb.linearVelocity, gravityDir);
        rb.linearVelocity = moveVelocity + gravityDir * gravitySpeed;
    }

    void Update()
    {
        HandleMovement();
    }
}