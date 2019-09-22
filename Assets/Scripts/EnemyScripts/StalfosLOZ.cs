using System;
using System.Collections;
using UnityEngine;

public class StalfosLOZ : MonoBehaviour
{
    [SerializeField] private GameObject _pfbEnemyExplosion = default;
    [SerializeField] private Rigidbody2D _stalfosRigidbody = default;
    private readonly int _knockbackForce = 10;
    private int _health = 2;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerAttack"))
        {
            Damaged();
            Knockback(other);
        }
    }

    private void Knockback(Collider2D other)
    {
        Vector2 direction = (transform.position - other.transform.position).normalized;
        Vector2 strictDirection = new Vector2(direction.x * Convert.ToInt32(Mathf.Abs(direction.x) > Mathf.Abs(direction.y)), direction.y * Convert.ToInt32(Mathf.Abs(direction.y) > Mathf.Abs(direction.x)));

        _stalfosRigidbody.velocity = strictDirection * _knockbackForce;
        StartCoroutine(ResetVelocity());
    }

    private void Damaged()
    {
        AudioManager.Instance.Play("EnemyDamaged(LOZ)");
        _health--;
        if (_health <= 0)
        {
            Died();
        }
    }

    private void Died()
    {
        AudioManager.Instance.Play("EnemyDied(LOZ)");
        Instantiate(_pfbEnemyExplosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    IEnumerator ResetVelocity()
    {
        yield return new WaitForSeconds(0.4f);
        _stalfosRigidbody.velocity = Vector2.zero;
    }
}
