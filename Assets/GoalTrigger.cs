using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    [Header("クリア設定")]
    public GameObject clearUI;

    private bool isCleared = false;

    void Start()
    {
        Collider col = GetComponent<Collider>();
        if (col == null)
            Debug.LogError("Colliderがありません！");
        else
            Debug.Log($"Collider確認OK！ IsTrigger:{col.isTrigger}");
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"触れたオブジェクト:{other.gameObject.name}");

        // 触れたオブジェクトから親をさかのぼってPlayerタグを探す
        Transform current = other.transform;
        bool isPlayer = false;

        while (current != null)
        {
            if (current.CompareTag("Player"))
            {
                isPlayer = true;
                break;
            }
            current = current.parent;
        }

        if (isPlayer && !isCleared)
        {
            isCleared = true;
            OnClear();
        }
    }

    void OnClear()
    {
        Debug.Log("クリア！");

        if (clearUI != null)
            clearUI.SetActive(true);
        else
            Debug.LogWarning("clearUIが設定されていません！");

        var playerMovement = FindObjectOfType<PlayerMovement>();
        if (playerMovement != null)
            playerMovement.enabled = false;

        var gravityManager = FindObjectOfType<GravityManager>();
        if (gravityManager != null)
            gravityManager.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}