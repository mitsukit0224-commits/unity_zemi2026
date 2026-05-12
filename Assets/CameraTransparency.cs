using UnityEngine;
using System.Collections.Generic;

public class CameraTransparency : MonoBehaviour
{
    public Transform player;
    public float transparentAlpha = 0.2f;

    private Dictionary<Renderer, Material[]> originalMaterials = new Dictionary<Renderer, Material[]>();
    private List<Renderer> currentTransparent = new List<Renderer>();

    void LateUpdate()
    {
        if (player == null) return;

        // 前フレームで透明にしたものを元に戻す
        foreach (var renderer in currentTransparent)
        {
            if (renderer != null && originalMaterials.ContainsKey(renderer))
                renderer.materials = originalMaterials[renderer];
        }
        currentTransparent.Clear();

        // カメラとプレイヤーの間にあるオブジェクトを検出
        Vector3 direction = player.position - transform.position;
        float distance = direction.magnitude;

        RaycastHit[] hits = Physics.RaycastAll(transform.position, direction.normalized, distance);

        foreach (var hit in hits)
        {
            Renderer renderer = hit.collider.GetComponent<Renderer>();
            if (renderer == null) continue;
            if (hit.collider.CompareTag("Player")) continue;

            // 元のマテリアルを保存
            if (!originalMaterials.ContainsKey(renderer))
                originalMaterials[renderer] = renderer.materials;

            // 透明マテリアルを作成して適用
            Material[] transparentMats = new Material[renderer.materials.Length];
            for (int i = 0; i < renderer.materials.Length; i++)
            {
                transparentMats[i] = new Material(renderer.materials[i]);
                transparentMats[i].SetFloat("_Surface", 1); // Transparent
                transparentMats[i].SetFloat("_Blend", 0);   // Alpha
                transparentMats[i].SetOverrideTag("RenderType", "Transparent");
                transparentMats[i].renderQueue = 3000;

                Color color = transparentMats[i].color;
                color.a = transparentAlpha;
                transparentMats[i].color = color;

                // URPのキーワードを有効化
                transparentMats[i].EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            }

            renderer.materials = transparentMats;
            currentTransparent.Add(renderer);
        }
    }
}