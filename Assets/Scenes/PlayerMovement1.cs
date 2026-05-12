using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement1 : MonoBehaviour
{
    [Header("── 移動 ──")]
    public float moveSpeed        = 5f;
    [Tooltip("加速の滑らかさ（小さいほどゆっくり加速）")]
    public float acceleration     = 12f;
    [Tooltip("入力ゼロ時の減速の滑らかさ（小さいほどスッと止まる）")]
    public float deceleration     = 16f;

    [Header("── 回転（Q/E）──")]
    public float rotationSpeed    = 100f;

    private Rigidbody rb;
    private Vector3   currentMoveVelocity = Vector3.zero; // 水平速度を補間管理

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        HandleRotation();
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleRotation()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        Vector3 gravityUp = -GravityManager.GravityDirection;

        if (keyboard.qKey.isPressed)
            transform.Rotate(gravityUp, -rotationSpeed * Time.deltaTime, Space.World);

        if (keyboard.eKey.isPressed)
            transform.Rotate(gravityUp,  rotationSpeed * Time.deltaTime, Space.World);
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

        // プレイヤーの向き基準で移動方向を計算
        Vector3 forward = Vector3.ProjectOnPlane(transform.forward, gravityDir).normalized;
        Vector3 right   = Vector3.ProjectOnPlane(transform.right,   gravityDir).normalized;

        Vector3 inputVelocity = (right * horizontal + forward * vertical).normalized * moveSpeed;

        // 入力があれば加速、なければ減速
        float lerpSpeed = (inputVelocity.sqrMagnitude > 0.01f) ? acceleration : deceleration;
        currentMoveVelocity = Vector3.Lerp(
            currentMoveVelocity, inputVelocity,
            lerpSpeed * Time.fixedDeltaTime);

        // 重力方向の速度は物理エンジンに任せてそのまま保持
        float gravitySpeed = Vector3.Dot(rb.linearVelocity, gravityDir);
        rb.linearVelocity  = currentMoveVelocity + gravityDir * gravitySpeed;
    }
}