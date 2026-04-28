using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GravityManager : MonoBehaviour
{
    [Header("Settings")]
    public float gravityMagnitude = 20f;

    public static Vector3 GravityDirection { get; private set; } = Vector3.down;

    private static List<Rigidbody> affectedBodies = new List<Rigidbody>();

    public static void Register(Rigidbody rb)
    {
        if (!affectedBodies.Contains(rb))
            affectedBodies.Add(rb);
    }

    public static void Unregister(Rigidbody rb)
    {
        affectedBodies.Remove(rb);
    }

    void Update()
    {
        HandleGravityInput();
    }

    void FixedUpdate()
    {
        foreach (var rb in affectedBodies)
        {
            if (rb != null)
                rb.AddForce(GravityDirection * gravityMagnitude, ForceMode.Acceleration);
        }
    }

    void HandleGravityInput()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        // プレイヤーの向きを取得（カメラ基準で相対的に重力を変える）
        Transform playerTransform = Camera.main.transform;

        if (keyboard.upArrowKey.wasPressedThisFrame)
            SetGravity(playerTransform.forward);   // 現在の前方向へ

        if (keyboard.downArrowKey.wasPressedThisFrame)
            SetGravity(-playerTransform.forward);  // 現在の後ろ方向へ

        if (keyboard.leftArrowKey.wasPressedThisFrame)
            SetGravity(-playerTransform.right);    // 現在の左方向へ

        if (keyboard.rightArrowKey.wasPressedThisFrame)
            SetGravity(playerTransform.right);     // 現在の右方向へ
    }

    void SetGravity(Vector3 newDirection)
    {
        // 最も近い軸にスナップ（斜めにならないように）
        newDirection = SnapToAxis(newDirection);
        GravityDirection = newDirection;

        foreach (var rb in affectedBodies)
        {
            if (rb != null)
            {
                float gravitySpeed = Vector3.Dot(rb.linearVelocity, GravityDirection);
                rb.linearVelocity -= GravityDirection * gravitySpeed;
            }
        }
    }

    // 最も近いワールド軸（上下左右前後）にスナップする
    Vector3 SnapToAxis(Vector3 direction)
    {
        Vector3[] axes = {
            Vector3.right, Vector3.left,
            Vector3.up,    Vector3.down,
            Vector3.forward, Vector3.back
        };

        Vector3 best = Vector3.down;
        float maxDot = float.MinValue;

        foreach (var axis in axes)
        {
            float dot = Vector3.Dot(direction, axis);
            if (dot > maxDot)
            {
                maxDot = dot;
                best = axis;
            }
        }

        return best;
    }
}