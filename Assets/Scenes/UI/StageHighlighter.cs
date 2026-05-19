using UnityEngine;

public class StageHighlighter : MonoBehaviour
{
    [Header("壁のRenderer")]
    public Renderer wallUp;
    public Renderer wallDown;
    public Renderer wallLeft;
    public Renderer wallRight;
    public Renderer wallForward;
    public Renderer wallBack;

    [Header("マテリアル")]
    public Material defaultMaterial;
    public Material highlightMaterial;

    public void Highlight(GravityFace face)
    {
        ResetAllWalls();

        Renderer target = face switch
        {
            GravityFace.Up      => wallUp,
            GravityFace.Down    => wallDown,
            GravityFace.Left    => wallLeft,
            GravityFace.Right   => wallRight,
            GravityFace.Forward => wallForward,
            GravityFace.Back    => wallBack,
            _                   => null
        };

        if (target != null)
            target.material = highlightMaterial;
    }

    void ResetAllWalls()
    {
        wallUp.material      = defaultMaterial;
        wallDown.material    = defaultMaterial;
        wallLeft.material    = defaultMaterial;
        wallRight.material   = defaultMaterial;
        wallForward.material = defaultMaterial;
        wallBack.material    = defaultMaterial;
    }
}