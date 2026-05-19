using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalTrigger : MonoBehaviour
{
    [Header("クリア設定")]
    public GameObject clearUI;
    public string nextSceneName = "";
    public float waitTime = 2f;

    [Header("色の設定")]
    public Color inactiveColor = new Color(1f, 1f, 1f, 0.3f);
    public Color activeColor   = new Color(0f, 1f, 0f, 1f);

    [Header("パーティクル設定")]
    public ParticleSystem goalParticle; // InspectorでGoalParticleをアサイン

    private bool isCleared = false;
    private bool isActive  = false;
    private Renderer goalRenderer;
    private Material goalMaterial;

    void Start()
    {
        goalRenderer = GetComponent<Renderer>();

        if (goalRenderer != null)
        {
            goalMaterial = new Material(goalRenderer.material);
            goalRenderer.material = goalMaterial;
        }

        SetGoalActive(false);
    }

    public void SetGoalActive(bool active)
    {
        isActive = active;

        if (goalMaterial != null)
        {
            if (active)
            {
                goalMaterial.color = activeColor;
                goalMaterial.EnableKeyword("_EMISSION");
                goalMaterial.SetColor("_EmissionColor", activeColor * 2f);
            }
            else
            {
                goalMaterial.color = inactiveColor;
                goalMaterial.DisableKeyword("_EMISSION");
            }
        }

        // パーティクルをボタンが押されたら再生
        if (goalParticle != null)
        {
            if (active)
            {
                goalParticle.gameObject.SetActive(true);
                goalParticle.Play();
                Debug.Log("パーティクル再生！");
            }
            else
            {
                goalParticle.gameObject.SetActive(false);
            }
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

        Invoke("LoadNextScene", waitTime);
    }

    void LoadNextScene()
    {
        if (string.IsNullOrEmpty(nextSceneName))
        {
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