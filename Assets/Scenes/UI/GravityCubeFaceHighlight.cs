using UnityEngine;

public class GravityCubeFaceHighlight : MonoBehaviour
{
    [Header("マテリアル設定")]
    public Material defaultMaterial;
    public Material selectedMaterial;

    private MeshRenderer meshRenderer;
    private GravityFace currentFace;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void SelectFace(GravityFace face)
    {
        currentFace = face;
        // 必要に応じてマテリアル切り替えを拡張
        meshRenderer.material = selectedMaterial;
    }

    public void ResetFace()
    {
        meshRenderer.material = defaultMaterial;
    }
}