using UnityEngine;
using UnityEngine.UI;

public class Room : MonoBehaviour
{
    private BoxCollider2D _roomCollider;
    void Start()
    {
        _roomCollider = GetComponent<BoxCollider2D>();
        CheckCharacters();
    }

    private void CheckCharacters()
    {
        Collider2D[] enemiesToDamage = Physics2D.OverlapBoxAll(transform.position, transform.position, 0);
        foreach  (var i in enemiesToDamage)
        {
            if (i.gameObject.gameObject.layer == LayerMask.NameToLayer("Character"))
                Debug.Log(i.name);
        }
    }
}
