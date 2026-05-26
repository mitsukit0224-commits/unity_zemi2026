using UnityEngine;
using UnityEngine.InputSystem;

public class GravityCubeRotator : MonoBehaviour
{
    public float rotateSpeed = 200f;
    public Camera uiCamera;

    private bool isDragging;
    private Vector2 lastMousePos;

    void Update()
    {
        var mouse = Mouse.current;
        if (mouse == null) return;

        Vector2 mousePos = mouse.position.ReadValue();

        // UICameraのエリア内かチェック
        Rect rect = uiCamera.rect;
        Vector2 viewportPos = new Vector2(
            (mousePos.x / Screen.width - rect.x) / rect.width,
            (mousePos.y / Screen.height - rect.y) / rect.height
        );

        bool isInUIArea = viewportPos.x >= 0 && viewportPos.x <= 1 &&
                          viewportPos.y >= 0 && viewportPos.y <= 1;

        if (mouse.leftButton.wasPressedThisFrame && isInUIArea)
        {
            isDragging = true;
            lastMousePos = mousePos;
        }
        if (mouse.leftButton.wasReleasedThisFrame)
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector2 delta = mousePos - lastMousePos;
            transform.Rotate(Vector3.up,    -delta.x * rotateSpeed * Time.deltaTime, Space.World);
            transform.Rotate(Vector3.right,  delta.y * rotateSpeed * Time.deltaTime, Space.World);
            lastMousePos = mousePos;
        }
    }
}