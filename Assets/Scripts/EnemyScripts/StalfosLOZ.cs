using System;
using System.Collections;
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
            Vector2 direction = (transform.position - other.transform.position).normalized;
            Vector2 strictDirection = new Vector2(direction.x * Convert.ToInt32(Mathf.Abs(direction.x) > Mathf.Abs(direction.y)), direction.y * Convert.ToInt32(Mathf.Abs(direction.y) > Mathf.Abs(direction.x)));
            Debug.LogError(strictDirection);

            _stalfosRigidbody.velocity = strictDirection * 4;
            StartCoroutine(ResetVelocity());
            //Damaged();
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

    IEnumerator ResetVelocity()
    {
        yield return new WaitForSeconds(0.5f);
        _stalfosRigidbody.velocity = Vector2.zero;
    }
}
