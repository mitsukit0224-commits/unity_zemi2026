using UnityEngine;

public class GravityObject : MonoBehaviour
{
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // デフォルト重力をオフ
        rb.freezeRotation = true;

        // GravityManagerに登録
        GravityManager.Register(rb);
    }

    void OnDestroy()
    {
        // オブジェクト削除時に登録解除
        GravityManager.Unregister(rb);
    }
}