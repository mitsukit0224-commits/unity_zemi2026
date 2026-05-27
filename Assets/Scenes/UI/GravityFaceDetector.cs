using UnityEngine;
using UnityEngine.InputSystem;

public class GravityFaceDetector : MonoBehaviour
{
    public Camera uiCamera;
    public StageHighlighter highlighter;
    public GravityManager gravityManager;

    private GravityFace? selectedFace = null;
    private int uiLayerMask;

    void Start()
    {
        uiLayerMask = LayerMask.GetMask("UI");
    }

    void Update()
    {
        var mouse = Mouse.current;
        if (mouse == null) return;
        if (!mouse.leftButton.wasPressedThisFrame) return;

        Vector2 mousePos = mouse.position.ReadValue();

        Rect rect = uiCamera.rect;
        Vector2 viewportPos = new Vector2(
            (mousePos.x / Screen.width - rect.x) / rect.width,
            (mousePos.y / Screen.height - rect.y) / rect.height
        );

        if (viewportPos.x < 0 || viewportPos.x > 1 ||
            viewportPos.y < 0 || viewportPos.y > 1) return;

        Ray ray = uiCamera.ViewportPointToRay(viewportPos);

        if (!Physics.Raycast(ray, out RaycastHit hit, 1000f, uiLayerMask)) return;

        // GravityFaceTagを取得
        GravityFaceTag faceTag = hit.collider.GetComponent<GravityFaceTag>();
        if (faceTag == null) return;

        GravityFace face = faceTag.face;
        Debug.Log("選択した面: " + face);

        if (selectedFace == face)
        {
            ApplyGravity(face);
            selectedFace = null;
            highlighter.ResetAllWalls();
        }
        else
        {
            selectedFace = face;
            highlighter.Highlight(face);
        }
    }

    void ApplyGravity(GravityFace face)
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