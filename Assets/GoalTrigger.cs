using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalTrigger : MonoBehaviour
{
    [Header("クリア設定")]
    public GameObject clearUI;

    [Header("次のステージ設定")]
    public string nextSceneName = "Stage2"; // 次のシーン名
    public float waitTime = 2f; // クリアUIを表示してから遷移するまでの秒数

    private bool isCleared = false;
    private bool isActive = false;

    void Start()
    {
        SetGoalActive(false);
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
        if (!isActive) return;

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

        var playerMovement = FindObjectOfType<PlayerMovement>();
        if (playerMovement != null)
            playerMovement.enabled = false;

        var gravityManager = FindObjectOfType<GravityManager>();
        if (gravityManager != null)
            gravityManager.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // 次のシーンへ遷移
        Invoke("LoadNextScene", waitTime);
    }

    void LoadNextScene()
    {
        if (string.IsNullOrEmpty(nextSceneName))
        {
            // シーン名が空の場合は次のインデックスへ
            int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
            if (nextIndex < SceneManager.sceneCountInBuildSettings)
                SceneManager.LoadScene(nextIndex);
            else
                Debug.Log("次のシーンがありません！");
        }
        else
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}