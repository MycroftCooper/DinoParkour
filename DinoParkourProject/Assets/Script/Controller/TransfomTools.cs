using System.Collections;
using UnityEngine;

public class TransfomTools : MonoBehaviour
{
    public delegate void TransfomCallBack();
    public Coroutine moveToPositionByCoroutine(Transform transform, Vector3 destroyPosition, float moveSpeed)
        => StartCoroutine(moveToPosition(transform, destroyPosition, moveSpeed, -1));
    public Coroutine moveToPositionByCoroutine(Transform transform, Vector3 destroyPosition, float moveSpeed, int inTime, TransfomCallBack callBack)
        => StartCoroutine(moveToPositionAndCallBack(transform, destroyPosition, moveSpeed, inTime, callBack));

    public void stopCoroutine(Coroutine c)
        => StopCoroutine(c);
    public IEnumerator moveToPosition(Transform transform, Vector3 targetPosition, float moveSpeed, int inTime)
    {
        float timer = 0;
        while (transform != null && transform.position != targetPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            timer +=Time.deltaTime;
            if (inTime != -1 && timer> inTime)
            {
                transform.position = targetPosition;
                yield break;
            }
            yield return 0;
        }
    }
    public IEnumerator moveToPositionAndCallBack(Transform transform, Vector3 targetPosition, float moveSpeed, int inTime, TransfomCallBack callBack)
    {
        yield return StartCoroutine(moveToPosition(transform, targetPosition, moveSpeed, inTime));
        callBack();
    }
}