using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    private BoxCollider2D _roomCollider;


    void Start()
    {
        _roomCollider = GetComponent<BoxCollider2D>();
    }

    public List<GameObject> GetEnemies()
    {
        List<GameObject> enemiesToReturn = new List<GameObject>();
        Collider2D[] enemies = Physics2D.OverlapBoxAll(transform.position, new Vector2(_roomCollider.size.x, _roomCollider.size.y), 0);
        foreach  (Collider2D enemy in enemies)
        {
            if (enemy.gameObject.CompareTag("Enemy"))
            {
                enemiesToReturn.Add(enemy.gameObject);
            }
        }
        return enemiesToReturn;
    }
}
