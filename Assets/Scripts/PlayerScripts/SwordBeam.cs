using UnityEngine;

public class SwordBeam : MonoBehaviour
{
    [SerializeField] private Rigidbody2D swordBeamRigidbody = default;
    [SerializeField] private GameObject _pfbSwordBeamExplosion = default;
    private readonly int speed = 70;


    void Start()
    {
        swordBeamRigidbody.AddForce(transform.right * speed * 10);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player"))
        {
            Instantiate(_pfbSwordBeamExplosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
