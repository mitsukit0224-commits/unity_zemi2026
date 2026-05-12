using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{
    [Header("設定")]
    public GoalTrigger goalTrigger;
    public Material pressedMaterial;

    private bool isPressed = false;

    void OnCollisionEnter(Collision other)
    {
        Debug.Log($"ボタンに触れた！ オブジェクト:{other.gameObject.name}");

        if (isPressed) return;

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