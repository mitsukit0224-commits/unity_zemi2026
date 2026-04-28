using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody rb;
    private CameraController cameraController;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        // CameraControllerを取得できているか確認
        cameraController = Camera.main.GetComponent<CameraController>();

        if (cameraController == null)
            Debug.LogError("CameraControllerが見つかりません！");
        else
            Debug.Log("CameraController取得成功！");
    }

    void Update()
    {
        HandleMovement();
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

        // 入力確認用ログ
        if (horizontal != 0 || vertical != 0)
            Debug.Log($"入力あり！ horizontal:{horizontal} vertical:{vertical}");

        Vector3 gravityDir = GravityManager.GravityDirection;

        // CameraControllerが取得できていない場合はCamera.main直接で代用
        Vector3 forward;
        Vector3 right;

        if (cameraController != null)
        {
            forward = cameraController.CameraForward;
            right   = cameraController.CameraRight;
        }
        else
        {
            // フォールバック
            forward = Vector3.ProjectOnPlane(Camera.main.transform.forward, gravityDir).normalized;
            right   = Vector3.ProjectOnPlane(Camera.main.transform.right,   gravityDir).normalized;
        }

        // 方向確認用ログ
        Debug.Log($"forward:{forward} right:{right}");

        Vector3 moveVelocity = (right * horizontal + forward * vertical) * moveSpeed;

        float gravitySpeed = Vector3.Dot(rb.linearVelocity, gravityDir);
        rb.linearVelocity = moveVelocity + gravityDir * gravitySpeed;
    }
}