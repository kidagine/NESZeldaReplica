﻿using System.Collections;
using UnityEngine;

public class Boomerang : MonoBehaviour
{
    [SerializeField] private GameObject _pfbImpactExplosion = default;
    [SerializeField] private Rigidbody2D _boomerangRigidbody = default;
    private GameObject _target;
    private readonly int _throwSpeed = 40;
    private readonly int _returnSpeed = 8;
    private bool _isReturning;

    void Start()
    {
        AudioManager.Instance.Play("Boomerang(LOZ)");
        _boomerangRigidbody.AddForce(transform.right * _throwSpeed * 10);
        StartCoroutine(ReturnBoomerang());
    }

    void FixedUpdate()
    {
        if (_isReturning)
        {
            _boomerangRigidbody.velocity = (_target.transform.position - transform.position).normalized * _returnSpeed;
        }
    }

    public void SetTarget(GameObject target)
    {
        _target = target;
    }

    IEnumerator ReturnBoomerang()
    {
        yield return new WaitForSeconds(0.35f);
        bool hasFlipped = false;
        float ratio = 0.0f;
        Vector2 targetVelocity = (_target.transform.position - transform.position).normalized * _returnSpeed;
        Vector2 startVelocity = _boomerangRigidbody.velocity;
        while(!hasFlipped)
        {
            if (ratio <= 1.0f)
            {
                _boomerangRigidbody.velocity = Vector2.Lerp(startVelocity, targetVelocity, ratio);
                ratio += 0.03f;
                yield return null;
            }
            else
            {
                hasFlipped = false;
                _isReturning = true;
                yield return null;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            AudioManager.Instance.Stop("Boomerang(LOZ)");
            LinkLOZ link = other.gameObject.GetComponent<LinkLOZ>();
            link.CatchBoomerang(gameObject);
        }
        else
        {
            Instantiate(_pfbImpactExplosion, transform.position, Quaternion.identity);
            _boomerangRigidbody.velocity = (_target.transform.position - transform.position).normalized * _returnSpeed;
        }
    }
}
