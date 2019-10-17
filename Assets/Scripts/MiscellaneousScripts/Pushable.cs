using System;
using System.Collections;
using UnityEngine;

public class Pushable : MonoBehaviour
{
    private readonly float _knockbackForce = 1.0f;


    public void Push(GameObject player)
    {
        Vector2 direction = (transform.position - player.transform.position).normalized;
        Vector2 strictDirection = new Vector2(Mathf.Round(direction.x * Convert.ToInt32(Mathf.Abs(direction.x) > Mathf.Abs(direction.y))), Mathf.Round(direction.y * Convert.ToInt32(Mathf.Abs(direction.y) > Mathf.Abs(direction.x))));

        StartCoroutine(PushToDirection(strictDirection));
    }

    IEnumerator PushToDirection(Vector2 pushDirection)
    {
        float ratio = 0.0f;
        Vector2 originalPosition = transform.position;
        Vector3 targetPosition = originalPosition + pushDirection;
        while(ratio != 1.0f)
        {
            transform.position = Vector2.Lerp(originalPosition, targetPosition, ratio);
            ratio += 0.1f;
            yield return null;
        }
    }
}
