using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{
    [Header("設定")]
    public GoalTrigger goalTrigger;
    public Material pressedMaterial;

    private bool isPressed = false;

    void OnTriggerEnter(Collider other)
    {
        if (isPressed) return;

        // 親をさかのぼってPlayerタグを探す
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

        // プレイヤーが触れた場合は反応しない
        if (isPlayer)
        {
            Debug.Log("プレイヤーが触れましたが反応しません！");
            return;
        }

        // プレイヤー以外のオブジェクトが触れた場合
        Debug.Log($"オブジェクトがボタンに触れました！:{other.gameObject.name}");
        isPressed = true;
        OnButtonPressed();
    }

    void OnButtonPressed()
    {
        Debug.Log("ボタンが押されました！");

        if (goalTrigger != null)
        {
            goalTrigger.SetGoalActive(true);
            Debug.Log("ゴールに有効化を送信しました！");
        }
        else
        {
            Debug.LogError("goalTriggerが設定されていません！");
        }

        if (pressedMaterial != null)
            GetComponent<Renderer>().material = pressedMaterial;
    }
}