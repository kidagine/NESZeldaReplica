using System;
using System.Collections;
using UnityEngine;

public class StalfosLOZ : MonoBehaviour, IEnemy
{
    [SerializeField] private GameObject _pfbEnemyExplosion = default;
    [SerializeField] private GameObject _pfbEnemySpawnExplosion = default;
    [SerializeField] private Rigidbody2D _stalfosRigidbody = default;
    private Vector2 _direction;
    private readonly int _moveSpeed = 3;
    private readonly int _attackDamage = 1;
    private readonly int _knockbackForce = 10;
    private int _health = 2;
    private float _randomWaitTime;


    void Start()
    {
        _direction = GetRandomDirection();
        MoveToDirection();
        StartCoroutine(ChooseRandomDirection());
    }

    IEnumerator ChooseRandomDirection()
    {
        _randomWaitTime = UnityEngine.Random.Range(0.4f, 1.3f);
        yield return new WaitForSeconds(_randomWaitTime);
        _direction = GetRandomDirection();
        MoveToDirection();
        StartCoroutine(ChooseRandomDirection());
    }

    private Vector2 GetRandomDirection()
    {
        int randomDirectionNumber = UnityEngine.Random.Range(1, 5);
        Vector2 randomDirection;
        switch (randomDirectionNumber)
        {
            case 1:
                randomDirection = new Vector2(1.0f, 0.0f);
                break;
            case 2:
                randomDirection = new Vector2(-1.0f, 0.0f);
                break;
            case 3:
                randomDirection = new Vector2(0.0f, 1.0f);
                break;
            case 4:
                randomDirection = new Vector2(0.0f, -1.0f);
                break;
            default:
                randomDirection = Vector2.zero;
                break;
        }
        if (randomDirection != _direction)
        {
            return randomDirection;
        }
        else
        {
            return GetRandomDirection();
        }
    }

    private void MoveToDirection()
    {
        _stalfosRigidbody.velocity = _direction * _moveSpeed;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            LinkLOZ link = other.gameObject.GetComponent<LinkLOZ>();
            link.Damage(_attackDamage, gameObject);
        }
        else
        {
            //Debug.Log("enter");
            //_randomWaitTime = UnityEngine.Random.Range(1.0f, 1.3f);
            //_direction = GetRandomDirection();
            //MoveToDirection();
        }
    }

    public void Damage(GameObject player, int attackDamage)
    {
        AudioManager.Instance.Play("EnemyDamaged(LOZ)");
        Knockback(player);
        _health -= attackDamage;
        if (_health <= 0)
        {
            Died();
        }
    }

    private void Knockback(GameObject player)
    {
        Vector2 direction = (transform.position - player.transform.position).normalized;
        Vector2 strictDirection = new Vector2(Mathf.Round(direction.x * Convert.ToInt32(Mathf.Abs(direction.x) > Mathf.Abs(direction.y))), Mathf.Round(direction.y * Convert.ToInt32(Mathf.Abs(direction.y) > Mathf.Abs(direction.x))));

        _stalfosRigidbody.velocity = strictDirection * _knockbackForce;
        StartCoroutine(ResetVelocity());
    }

    public void Stun()
    {
        StartCoroutine(StunTimer());
    }

    IEnumerator StunTimer()
    {
        _stalfosRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForSeconds(3.0f);
        _stalfosRigidbody.constraints = RigidbodyConstraints2D.None;
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

    public void Spawn()
    {
        StartCoroutine(SpawnTimer());
    }

    IEnumerator SpawnTimer()
    {
        Instantiate(_pfbEnemySpawnExplosion, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.13f);
        GetComponent<SpriteRenderer>().enabled = true;
        yield return null;
    }

    public void Hide()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }
}
