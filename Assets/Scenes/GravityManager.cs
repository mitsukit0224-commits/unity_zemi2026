using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GravityManager : MonoBehaviour
{
    [Header("Settings")]
    public float gravityMagnitude = 20f;

    public static Vector3 GravityDirection { get; private set; } = Vector3.down;

    private static List<Rigidbody> affectedBodies = new List<Rigidbody>();

    void Start()
    {
        GravityDirection = Vector3.down;
        //affectedBodies.Clear();
    }

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

        if (Camera.main == null) return;

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
            if (rb == null) continue;

            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            PushOutFromCollision(rb);
        }
    }

    void PushOutFromCollision(Rigidbody rb)
    {
        Collider[] colliders = rb.GetComponents<Collider>();

        foreach (var col in colliders)
        {
            Collider[] overlapping = Physics.OverlapBox(
                rb.position,
                col.bounds.extents * 0.9f,
                rb.rotation
            );

            foreach (var other in overlapping)
            {
                if (other.attachedRigidbody == rb) continue;
                if (other.isTrigger) continue;

                Vector3 direction;
                float distance;

                if (Physics.ComputePenetration(
                    col, rb.position, rb.rotation,
                    other, other.transform.position, other.transform.rotation,
                    out direction, out distance))
                {
                    rb.position += direction * (distance + 0.01f);
                }
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