using UnityEngine;

public class GravityCubeRotator : MonoBehaviour
{
    public float rotateSpeed = 100f;
    public Camera uiCamera;

    private bool isDragging;
    private Vector3 lastMousePos;

    void Update()
    {
        // キューブの上にマウスがあるときだけドラッグ有効
        Ray ray = uiCamera.ScreenPointToRay(Input.mousePosition);
        bool isOverCube = Physics.Raycast(ray, out RaycastHit hit)
                          && hit.collider.gameObject == gameObject;

        if (Input.GetMouseButtonDown(0) && isOverCube)
        {
            isDragging = true;
            lastMousePos = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector3 delta = Input.mousePosition - lastMousePos;
            transform.Rotate(Vector3.up,    -delta.x * rotateSpeed * Time.deltaTime, Space.World);
            transform.Rotate(Vector3.right,  delta.y * rotateSpeed * Time.deltaTime, Space.World);
            lastMousePos = Input.mousePosition;
        }
        else
        {
            // 自動でゆっくり回転
            transform.Rotate(Vector3.up, 20f * Time.deltaTime, Space.World);
        }
    }
}