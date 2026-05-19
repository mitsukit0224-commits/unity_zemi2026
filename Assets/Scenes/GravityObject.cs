using UnityEngine;

public class GravityObject : MonoBehaviour
{
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.freezeRotation = true;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        GravityManager.Register(rb);
    }

    void OnDestroy()
    {
        GravityManager.Unregister(rb);
    }
}