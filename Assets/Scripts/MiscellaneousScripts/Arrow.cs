using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _arrowRigidbody = default;
    [SerializeField] private GameObject _pfbArrowExplosion = default;
    private readonly int speed = 70;


    void Start()
    {
        _arrowRigidbody.AddForce(transform.right * speed * 10);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player"))
        {
            Instantiate(_pfbArrowExplosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
