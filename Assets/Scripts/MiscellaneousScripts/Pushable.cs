using System;
using System.Collections;
using UnityEngine;

public class Pushable : MonoBehaviour
{
    private enum PushableSide { Top, Bottom, Left, Right }
    [SerializeField] private PushableSide _pushable = default;
    [SerializeField] private GameObject _linkedObject = default;

    public void Push(GameObject player)
    {
        Vector2 direction = (transform.position - player.transform.position).normalized;
        Vector2 strictDirection = new Vector2(Mathf.Round(direction.x * Convert.ToInt32(Mathf.Abs(direction.x) > Mathf.Abs(direction.y))), Mathf.Round(direction.y * Convert.ToInt32(Mathf.Abs(direction.y) > Mathf.Abs(direction.x))));

        CheckPushableSide(strictDirection);
    }

    private void CheckPushableSide(Vector2 pushDirection)
    {
        switch (_pushable)
        {
            case PushableSide.Top:
                if (pushDirection.y == -1)
                {
                    StartCoroutine(PushToDirection(pushDirection));
                }
                break;
            case PushableSide.Bottom:
                if (pushDirection.y == 1)
                {
                    StartCoroutine(PushToDirection(pushDirection));
                }
                break;
            case PushableSide.Left:
                if (pushDirection.x == -1)
                {
                    StartCoroutine(PushToDirection(pushDirection));
                }
                break;
            case PushableSide.Right:
                if (pushDirection.x == 1)
                {
                    StartCoroutine(PushToDirection(pushDirection));
                }
                break;
        }
    }

    IEnumerator PushToDirection(Vector2 pushDirection)
    {
        bool pushedObject = false;
        float ratio = 0.0f;
        Vector2 originalPosition = transform.position;
        Vector3 targetPosition = originalPosition + pushDirection;
        while (!pushedObject)
        {
            if (ratio <= 1.0f)
            {
                transform.position = Vector2.Lerp(originalPosition, targetPosition, ratio);
                ratio += 0.05f;
                yield return null;
            }
            else
            {
                if (_linkedObject != null)
                {
                    IInteractable interactable = _linkedObject.GetComponent<IInteractable>();
                    interactable.Interact();
                }
                pushedObject = true;
                yield return null;
            }
        }
    }
}
