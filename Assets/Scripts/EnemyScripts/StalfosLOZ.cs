using UnityEngine;

public class StalfosLOZ : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _stalfosRigidbody = default;
    private int _health = 2;

    Vector2 test = new Vector2(-2, 0);

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerAttack"))
        {
            Vector2 difference = transform.position - other.transform.position;
            _stalfosRigidbody.velocity = difference;
            Damaged();
        }
    }

    private void Damaged()
    {
        _health--;
        if (_health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
