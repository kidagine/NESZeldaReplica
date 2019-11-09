using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera2D : MonoBehaviour
{
    public static Camera2D Instance { get; private set; }
    private Room _currentRoom;


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

    public void SetCurrentRoom(Room currentRoom)
    {
        _currentRoom = currentRoom;
    }

    public void RoomTransition(DoorPosition doorPosition, GameObject player)
    {
        StartCoroutine(RoomTransitionTimer(doorPosition, player));
    }

    IEnumerator RoomTransitionTimer(DoorPosition doorPosition, GameObject player)
    {
        bool hasTransitioned = false;
        float ratio = 0.0f;
        Vector3 startingPosition = transform.position;
        Vector3 targetPosition = GetTransitionPosition(doorPosition);
        while (!hasTransitioned)
        {
            if (ratio <= 1.0f)
            {
                transform.position = Vector3.Lerp(startingPosition, targetPosition, ratio);
                ratio += 0.01f;
                yield return null;
            }
            else
            {
                PlayerEnterRoom(player, doorPosition);
                SpawnEnemies();
                hasTransitioned = true;
            }
        }
    }

    private Vector3 GetTransitionPosition(DoorPosition doorPosition)
    {
        Vector3 transitionPosition = new Vector3();
        switch (doorPosition)
        {
            case DoorPosition.Bottom:
                transitionPosition = new Vector3(transform.position.x, transform.position.y + 11, transform.position.z);
                break;
            case DoorPosition.Top:
                transitionPosition = new Vector3(transform.position.x, transform.position.y - 11, transform.position.z);
                break;
            case DoorPosition.Left:
                transitionPosition = new Vector3(transform.position.x + 16, transform.position.y, transform.position.z);
                break;
            case DoorPosition.Right:
                transitionPosition = new Vector3(transform.position.x - 16, transform.position.y, transform.position.z);
                break;
        }
        return transitionPosition;
    }

    private void PlayerEnterRoom(GameObject player, DoorPosition doorPosition)
    {
        player.SetActive(true);
        Vector2 playerDirection = Vector2.zero;
        switch (doorPosition)
        {
            case DoorPosition.Bottom:
                playerDirection = new Vector2(0, 1);
                break;
            case DoorPosition.Top:
                playerDirection = new Vector2(0, -1);
                break;
            case DoorPosition.Left:
                playerDirection = new Vector2(1, 0);
                break;
            case DoorPosition.Right:
                playerDirection = new Vector2(-1, 0);
                break;
        }
        player.transform.GetChild(0).GetComponent<LinkAnimationEvents>().WalkToRoom(playerDirection);
    }

    private void SpawnEnemies()
    {
        List<GameObject> enemies = _currentRoom.GetEnemies();
        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<IEnemy>().Spawn();
        }
    }
}
