using UnityEngine;
using UnityEngine.InputSystem;

public class GravityController : MonoBehaviour
{
    [Header("Settings")]
    public float gravityMagnitude = 20f;  // 重力を強くした
    public float rotationSpeed = 10f;
    public float moveSpeed = 5f;

    private Rigidbody rb;
    private Vector3 gravityDirection = Vector3.down;
    private Quaternion targetRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        targetRotation = transform.rotation;
    }

    void Update()
    {
        HandleGravityInput();
        HandleMovement();
        SmoothRotation();
    }

    void FixedUpdate()
    {
        rb.AddForce(gravityDirection * gravityMagnitude, ForceMode.Acceleration);
    }

    void HandleGravityInput()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        if (keyboard.upArrowKey.wasPressedThisFrame)    SetGravity(Vector3.back);
        if (keyboard.downArrowKey.wasPressedThisFrame)  SetGravity(Vector3.forward);
        if (keyboard.leftArrowKey.wasPressedThisFrame)  SetGravity(Vector3.left);
        if (keyboard.rightArrowKey.wasPressedThisFrame) SetGravity(Vector3.right);
        if (keyboard.spaceKey.wasPressedThisFrame)      SetGravity(Vector3.down);
        if (keyboard.fKey.wasPressedThisFrame)          SetGravity(Vector3.up);
    }

    void SetGravity(Vector3 newDirection)
    {
        gravityDirection = newDirection;
        targetRotation = Quaternion.FromToRotation(Vector3.down, gravityDirection);

        // 重力方向の速度だけリセット（急な切り替え時の安定化）
        Vector3 vel = rb.linearVelocity;
        float dot = Vector3.Dot(vel, gravityDirection);
        rb.linearVelocity = vel - gravityDirection * dot;
    }

    void SmoothRotation()
    {
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * rotationSpeed
        );
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

        // 重力方向に対して垂直な平面での移動を計算
        // プレイヤーのローカル軸を使うことでどの壁でも正しく動く
        Vector3 right   = transform.right;
        Vector3 forward = transform.forward;

        // 重力方向の成分を除去して平面移動にする
        right   = (right   - gravityDirection * Vector3.Dot(right,   gravityDirection)).normalized;
        forward = (forward - gravityDirection * Vector3.Dot(forward, gravityDirection)).normalized;

        Vector3 moveVelocity = (right * horizontal + forward * vertical) * moveSpeed;

        // 重力方向の速度は維持しつつ、移動速度だけ上書き
        float gravitySpeed = Vector3.Dot(rb.linearVelocity, gravityDirection);
        rb.linearVelocity = moveVelocity + gravityDirection * gravitySpeed;
    }
}