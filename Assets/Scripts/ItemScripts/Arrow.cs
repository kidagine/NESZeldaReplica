using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _arrowRigidbody = default;
    [SerializeField] private GameObject _pfbImpactExplosion = default;
    private readonly int _speed = 60;


    void Start()
    {
        _arrowRigidbody.AddForce(transform.right * _speed * 10);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player"))
        {
            Instantiate(_pfbImpactExplosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
