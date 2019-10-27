using System.Collections;
using UnityEngine;

public class Camera2D : MonoBehaviour
{
    public static Camera2D Instance { get; private set; }


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        CheckIfVisible();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            CheckIfVisible();
        }
    }

    private void CheckIfVisible()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject go in allObjects)
        {
            if (go.gameObject.CompareTag("Door"))
            {
                if (go.gameObject.GetComponent<SpriteRenderer>().isVisible == false)
                {
                    go.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                }
                else
                {
                    go.gameObject.GetComponent<BoxCollider2D>().enabled = true;
                }
            }
        }        
    }

    public void RoomTransition(Vector2 direction)
    {
        StartCoroutine(RoomTransitionTimer(direction));
    }

    IEnumerator RoomTransitionTimer(Vector2 direction)
    {
        float ratio = 0.0f;
        Vector3 startingPosition = transform.position;
        Vector3 targetPosition = GetTransitionPosition(direction);
        while (ratio <= 1.0f)
        {
            transform.position = Vector3.Lerp(startingPosition, targetPosition, ratio);
            ratio += 0.01f;
            yield return null;
        }
    }

    private Vector3 GetTransitionPosition(Vector2 direction)
    {
        if (direction.x == 1.0f)
        {
            return new Vector3(transform.position.x + 16, transform.position.y, transform.position.z);
        }
        else if (direction.x == -1.0f)
        {
            return new Vector3(transform.position.x - 16, transform.position.y, transform.position.z);
        }
        else if (direction.y == 1.0f)
        {
            return new Vector3(transform.position.x, transform.position.y + 11, transform.position.z);
        }
        else
        {
            return new Vector3(transform.position.x, transform.position.y - 11, transform.position.z);
        }
    }
}
