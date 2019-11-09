using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomConnector : MonoBehaviour
{
    [SerializeField] private Room _room = default;
    private readonly List<GameObject> _doors = new List<GameObject>();


    void Start()
    {
        ConnectDoors();
    }

    private void ConnectDoors()
    {
        foreach (Transform child in transform)
        {
            _doors.Add(child.gameObject);
        }
        _doors[0].GetComponent<Door>().SetConnectedDoor(_doors[1]);
        _doors[1].GetComponent<Door>().SetConnectedDoor(_doors[0]);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameObject enteredDoor = GetEnteredDoor(other.gameObject);

            Camera2D.Instance.SetCurrentRoom(_room);
            other.gameObject.transform.position = enteredDoor.transform.position;
            other.gameObject.SetActive(false);
            List<GameObject> enemies = _room.GetEnemies();
            foreach (GameObject enemy in enemies)
            {
                enemy.GetComponent<IEnemy>().Hide();
            }

            Camera2D.Instance.RoomTransition(enteredDoor.GetComponent<Door>().DoorPosition, other.gameObject);
        }
    }

    private GameObject GetEnteredDoor(GameObject player)
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;
        Vector2 strictDirection = new Vector2(Mathf.Round(direction.x * Convert.ToInt32(Mathf.Abs(direction.x) > Mathf.Abs(direction.y))), Mathf.Round(direction.y * Convert.ToInt32(Mathf.Abs(direction.y) > Mathf.Abs(direction.x))));
        foreach (GameObject doorObject in _doors)
        {
            Door door = doorObject.GetComponent<Door>();
            if (!((int)strictDirection.y == 1 && door.DoorPosition == DoorPosition.Bottom))
            {
                if (!((int)strictDirection.y == -1 && door.DoorPosition == DoorPosition.Top))
                {
                    if (!((int)strictDirection.x == 1 && door.DoorPosition == DoorPosition.Left))
                    {
                        if (!((int)strictDirection.x == -1 && door.DoorPosition == DoorPosition.Right))
                        {
                            return doorObject;
                        }
                    }
                }
            }
        }
        return null;
    }
}
