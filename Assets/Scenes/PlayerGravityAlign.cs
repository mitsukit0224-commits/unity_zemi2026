using UnityEngine;

public class PlayerGravityAlign : MonoBehaviour
{
    public float rotationSpeed = 10f;

    void Update()
    {
        AlignToGravity();
    }

    void AlignToGravity()
    {
        Vector3 gravityUp = -GravityManager.GravityDirection;

        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, gravityUp) * transform.rotation;

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * rotationSpeed
        );
    }
}