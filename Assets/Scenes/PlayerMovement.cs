using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 100f; // Q/Eの回転速度

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        HandleMovement();
        HandleRotation();
    }

    void HandleRotation()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        Vector3 gravityUp = -GravityManager.GravityDirection;

        // Q/Eで重力の上方向を軸にプレイヤーを回転
        if (keyboard.qKey.isPressed)
            transform.Rotate(gravityUp, -rotationSpeed * Time.deltaTime, Space.World);

        if (keyboard.eKey.isPressed)
            transform.Rotate(gravityUp, rotationSpeed * Time.deltaTime, Space.World);
    }

    void HandleMovement()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        float horizontal = 0f;
        float vertical   = 0f;

        if (keyboard.aKey.isPressed) horizontal = -1f;
        if (keyboard.dKey.isPressed) horizontal =  1f;
        if (keyboard.wKey.isPressed) vertical   =  1f;
        if (keyboard.sKey.isPressed) vertical   = -1f;

        Vector3 gravityDir = GravityManager.GravityDirection;

        // プレイヤー自身の向きを基準に移動
        Vector3 forward = Vector3.ProjectOnPlane(transform.forward, gravityDir).normalized;
        Vector3 right   = Vector3.ProjectOnPlane(transform.right,   gravityDir).normalized;

        Vector3 moveVelocity = (right * horizontal + forward * vertical) * moveSpeed;

        float gravitySpeed = Vector3.Dot(rb.linearVelocity, gravityDir);
        rb.linearVelocity = moveVelocity + gravityDir * gravitySpeed;
    }
}