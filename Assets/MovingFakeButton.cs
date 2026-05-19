using UnityEngine;

public class MovingFakeButton : MonoBehaviour
{
    [Header("移動設定")]
    public Vector3 pointA = Vector3.zero;
    public Vector3 pointB = Vector3.right * 3f;
    public float moveSpeed = 2f;

    [Header("一緒に動かすオブジェクト")]
    public Transform[] linkedObjects;

    [Header("死亡設定")]
    public GameObject deathUI;

    private Vector3 startWorldPosition;
    private bool isDead = false;

    void Start()
    {
        startWorldPosition = transform.position;
        pointA = startWorldPosition + pointA;
        pointB = startWorldPosition + pointB;
    }

    void Update()
    {
        float t = (Mathf.Sin(Time.time * moveSpeed) + 1f) / 2f;
        Vector3 movement = Vector3.Lerp(pointA, pointB, t);
        Vector3 delta = movement - transform.position;

        transform.position = movement;

        foreach (var obj in linkedObjects)
        {
            if (obj != null)
                obj.position += delta;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (isDead) return;

        Debug.Log($"偽ボタンに触れた！:{other.gameObject.name} Tag:{other.tag}");

        // プレイヤーかどうか確認
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

        // プレイヤーは無視
        if (isPlayer)
        {
            Debug.Log("プレイヤーなので無視！");
            return;
        }

        // オブジェクトが触れたらプレイヤーを死亡させる
        Debug.Log("オブジェクトが触れました！プレイヤー死亡！");
        isDead = true;
        KillPlayer();
    }

    void KillPlayer()
    {
        Debug.Log("プレイヤーが死亡！");

        if (deathUI != null)
            deathUI.SetActive(true);
        else
            Debug.LogWarning("deathUIが設定されていません！");

        var playerMovement = FindObjectOfType<PlayerMovement>();
        if (playerMovement != null)
            playerMovement.enabled = false;

        var gravityManager = FindObjectOfType<GravityManager>();
        if (gravityManager != null)
            gravityManager.enabled = false;

        Invoke("Restart", 2f);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Restart()
    {
        UnityEngine.SceneManagement.SceneManager
            .LoadScene(UnityEngine.SceneManagement.SceneManager
            .GetActiveScene().name);
    }
}