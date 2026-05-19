using UnityEngine;

public class GravityFaceDetector : MonoBehaviour
{
    public Camera uiCamera;
    public StageHighlighter highlighter;
    public GravityManagerUI gravityManager;

    void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        Ray ray = uiCamera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit)) return;
        if (hit.collider.gameObject != gameObject) return;

        GravityFace face = GetFaceDirection(hit.normal);
        highlighter.Highlight(face);
        OnFaceClicked(face);
    }

    GravityFace GetFaceDirection(Vector3 normal)
    {
        Vector3 localNormal = transform.InverseTransformDirection(normal);

        if (localNormal.y >  0.9f) return GravityFace.Up;
        if (localNormal.y < -0.9f) return GravityFace.Down;
        if (localNormal.x >  0.9f) return GravityFace.Right;
        if (localNormal.x < -0.9f) return GravityFace.Left;
        if (localNormal.z >  0.9f) return GravityFace.Forward;
                                    return GravityFace.Back;
    }

    void OnFaceClicked(GravityFace face)
    {
        Vector3 dir = face switch
        {
            GravityFace.Up      => Vector3.up,
            GravityFace.Down    => Vector3.down,
            GravityFace.Left    => Vector3.left,
            GravityFace.Right   => Vector3.right,
            GravityFace.Forward => Vector3.forward,
            GravityFace.Back    => Vector3.back,
            _                   => Vector3.down
        };

        gravityManager.SetGravity(dir);
    }
}