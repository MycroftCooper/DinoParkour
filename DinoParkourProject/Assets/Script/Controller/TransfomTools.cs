using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransfomTools : MonoBehaviour
{
    public Coroutine moveToPositionByCoroutine(Transform transform, Vector3 destroyPosition, float moveSpeed)
        => StartCoroutine(moveToPosition(transform, destroyPosition, moveSpeed));
    public void stopCoroutine(Coroutine c)
        => StopCoroutine(c);
    public IEnumerator moveToPosition(Transform transform, Vector3 targetPosition, float moveSpeed)
    {
        while (transform != null && transform.position != targetPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return 0;
        }
    }
}