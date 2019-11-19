using System;
using System.Collections;
using UnityEngine;

public class StalfosLOZ : MonoBehaviour, IEnemy
{
    [SerializeField] private GameObject _pfbEnemyExplosion = default;
    [SerializeField] private GameObject _pfbEnemySpawnExplosion = default;
    [SerializeField] private Rigidbody2D _stalfosRigidbody = default;
    private Vector2 _direction;
    private Vector2 _initialPosition;
    private Vector2 _tempPosition;
    private readonly int _moveSpeed = 2;
    private readonly int _attackDamage = 1;
    private readonly int _knockbackForce = 10;
    private int _health = 2;
    private float _randomWaitTime;
    private bool _cantMove;
    private bool _hasCollided = false;


    void Start()
    {
        _initialPosition = transform.position;
        //Hide();
        _direction = GetRandomDirection();
        int randomStepsUntilChange = UnityEngine.Random.Range(5, 10);
        //StartCoroutine(ChooseRandomDirection(randomStepsUntilChange));
    }

    void Update()
    {
        float step = _moveSpeed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + _direction.x, transform.position.y + _direction.y), step);
    }

    void FixedUpdate()
    {
        Vector2 raycastPosition = new Vector2(transform.position.x + _direction.x, transform.position.y + _direction.y);
        Debug.DrawRay(raycastPosition, _direction, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(raycastPosition, _direction, 1.0f);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
            {
                _direction = GetRandomDirection();
            }
        }
    }

    IEnumerator ChooseRandomDirection(int randomStepsUntilChange)
    {
        //float ratio = 0.0f;
        //_tempPosition = transform.position;
        //if (randomStepsUntilChange != 0)
        //{
        //    while (ratio < 1.1f)
        //    {
        //        if (_hasCollided)
        //        {
        //            _direction = GetRandomDirection();
        //            randomStepsUntilChange = UnityEngine.Random.Range(5, 10);
        //            StartCoroutine(ChooseRandomDirection(randomStepsUntilChange));
        //            _hasCollided = false;
        //            Debug.Log("STOP");
        //            yield break;
        //        }
        //        transform.position = Vector2.Lerp(_tempPosition, new Vector2(_tempPosition.x + _direction.x, _tempPosition.y + _direction.y), ratio);
        //        ratio += 0.05f;
        //        yield return null;
        //    }
        //    randomStepsUntilChange--;
        //    StartCoroutine(ChooseRandomDirection(randomStepsUntilChange));
        //}
        //else
        //{
        //    _direction = GetRandomDirection();
        //    randomStepsUntilChange = UnityEngine.Random.Range(5, 10);
        //    StartCoroutine(ChooseRandomDirection(randomStepsUntilChange));
        //}
        yield return null;
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

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            LinkLOZ link = other.gameObject.GetComponent<LinkLOZ>();
            link.Damage(_attackDamage, gameObject);
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
        _cantMove = true;
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
        _cantMove = false;
        _stalfosRigidbody.velocity = Vector2.zero;
    }

    public void Spawn()
    {
        StartCoroutine(SpawnTimer());
    }

    IEnumerator SpawnTimer()
    {
        transform.position = _initialPosition;
        Instantiate(_pfbEnemySpawnExplosion, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.13f);
        GetComponent<SpriteRenderer>().enabled = true;
        _cantMove = false;
        _direction = GetRandomDirection();
    }

    public void Hide()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        _cantMove = true;
    }
}
