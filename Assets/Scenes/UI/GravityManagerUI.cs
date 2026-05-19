using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GravityManagerUI : MonoBehaviour
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

        Transform playerTransform = Camera.main.transform;

        if (keyboard.upArrowKey.wasPressedThisFrame)
            SetGravity(playerTransform.forward);
        if (keyboard.downArrowKey.wasPressedThisFrame)
            SetGravity(-playerTransform.forward);
        if (keyboard.leftArrowKey.wasPressedThisFrame)
            SetGravity(-playerTransform.right);
        if (keyboard.rightArrowKey.wasPressedThisFrame)
            SetGravity(playerTransform.right);
        if (keyboard.fKey.wasPressedThisFrame)
            SetGravity(Vector3.up);
        if (keyboard.spaceKey.wasPressedThisFrame)
            SetGravity(Vector3.down);
    }

    public void SetGravity(Vector3 newDirection)
    {
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