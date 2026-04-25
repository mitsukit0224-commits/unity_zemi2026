using System.Collections;
using UnityEngine;

public class CubeMover : MonoBehaviour
{
    [Header("移動設定")]
    public float moveDistance = 2f;   // 移動距離
    public float moveSpeed = 3f;      // 移動速度
    public int repeatCount = 3;       // 繰り返し回数

    void Start()
    {
        StartCoroutine(MoveSequence());
    }

    IEnumerator MoveSequence()
    {
        for (int i = 0; i < repeatCount; i++)
        {
            Debug.Log($"--- {i + 1}回目 ---");

            yield return StartCoroutine(Move(Vector3.up,    moveDistance)); // 上
            yield return StartCoroutine(Move(Vector3.right, moveDistance)); // 右
            yield return StartCoroutine(Move(Vector3.down,  moveDistance)); // 下
            yield return StartCoroutine(Move(Vector3.left,  moveDistance)); // 左
        }

        Debug.Log("完了！");
    }

    IEnumerator Move(Vector3 direction, float distance)
    {
        Vector3 startPos = transform.position;
        Vector3 targetPos = startPos + direction * distance;

        while (Vector3.Distance(transform.position, targetPos) > 0.001f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPos,
                moveSpeed * Time.deltaTime
            );
            yield return null;
        }

        transform.position = targetPos; // 誤差補正
    }
}