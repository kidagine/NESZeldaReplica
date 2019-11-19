using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    private BoxCollider2D _roomCollider;


    void Start()
    {
        _roomCollider = GetComponent<BoxCollider2D>();
        GetEnemies();
    }

    public List<GameObject> GetEnemies()
    {
        List<GameObject> enemiesToReturn = new List<GameObject>();
        Collider2D[] objectsInRoom = Physics2D.OverlapBoxAll(transform.position, new Vector2(_roomCollider.size.x, _roomCollider.size.y), 0);
        foreach  (Collider2D objectInRoom in objectsInRoom)
        {
            if (objectInRoom.gameObject.CompareTag("Enemy"))
            {
                enemiesToReturn.Add(objectInRoom.gameObject);
            }
            else if (objectInRoom.gameObject.CompareTag("Door"))
            {
                objectInRoom.GetComponent<Door>().ConnectedRoom = this;
            }
        }
        return enemiesToReturn;
    }
}
