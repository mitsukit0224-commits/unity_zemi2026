using UnityEngine;
using System.Collections.Generic;

public class CameraTransparency : MonoBehaviour
{
    public Transform player;
    [Range(0f, 1f)]
    public float transparentAlpha = 0.3f;
    public Material transparentMaterial; // InspectorでTransparentMatをアサイン

    private Dictionary<Renderer, Material[]> originalMaterials = new Dictionary<Renderer, Material[]>();
    private List<Renderer> currentTransparent = new List<Renderer>();

    void LateUpdate()
    {
        Vector3 direction = player.position - transform.position;
        float distance = direction.magnitude;
        RaycastHit[] hits = Physics.RaycastAll(transform.position, direction.normalized, distance);

        List<Renderer> hitRenderers = new List<Renderer>();

        foreach (var hit in hits)
        {
            Renderer rend = hit.collider.GetComponent<Renderer>();
            if (rend == null) continue;
            if (hit.collider.transform == player) continue;

            hitRenderers.Add(rend);

            if (!currentTransparent.Contains(rend))
            {
                MakeTransparent(rend);
                currentTransparent.Add(rend);
            }
        }

        List<Renderer> toRestore = new List<Renderer>();
        foreach (var rend in currentTransparent)
        {
            if (!hitRenderers.Contains(rend))
            {
                RestoreOpaque(rend);
                toRestore.Add(rend);
            }
        }
        foreach (var rend in toRestore)
        {
            currentTransparent.Remove(rend);
        }
    }

    void MakeTransparent(Renderer rend)
    {
        originalMaterials[rend] = rend.sharedMaterials;

        // TransparentMatに差し替え
        Material[] newMats = new Material[rend.sharedMaterials.Length];
        for (int i = 0; i < newMats.Length; i++)
        {
            newMats[i] = transparentMaterial;
        }
        rend.materials = newMats;
    }

    void RestoreOpaque(Renderer rend)
    {
        if (originalMaterials.ContainsKey(rend))
        {
            rend.materials = originalMaterials[rend];
            originalMaterials.Remove(rend);
        }
    }
}