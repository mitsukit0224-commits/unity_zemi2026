using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    [Header("クリア設定")]
    public GameObject clearUI;

    private bool isCleared = false;
    private bool isActive = false;

    void Start()
    {
        SetGoalActive(false);
        Debug.Log("ゴール初期化！無効状態でスタート");
    }

    public void SetGoalActive(bool active)
    {
        isActive = active;
        Debug.Log($"ゴール状態変更！ isActive:{isActive}");

        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            Color color = renderer.material.color;
            color.a = active ? 1f : 0.3f;
            renderer.material.color = color;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"ゴールに触れた！ オブジェクト:{other.gameObject.name} isActive:{isActive}");

        if (!isActive)
        {
            Debug.Log("ゴールがまだ無効です！");
            return;
        }

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

        Debug.Log($"プレイヤー判定:{isPlayer}");

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