using System.Collections;
using UnityEngine;

public class LinkLOZMovement : MonoBehaviour
{
    [SerializeField] private Animator _linkAnimator = default;
    [SerializeField] private Rigidbody2D _linkRigidbody = default;
    private Vector2 _direction;
    private Vector2 _lastDirection;
    private bool _isAttacking;
    private readonly int _moveSpeed = 3;


    void Update()
    {
        CheckPlayerDirection();
        Attack();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void CheckPlayerDirection()
    {
        if (!_isAttacking)
        {
            if (_direction.y != 1 && _direction.y != -1)
            {
                _direction.x = Mathf.RoundToInt(Input.GetAxisRaw("Horizontal"));
                if (_direction.x != 0)
                {
                    _linkAnimator.SetFloat("Vertical", 0);

                    _lastDirection.x = _direction.x;
                    _linkAnimator.SetFloat("Horizontal", _lastDirection.x);
                    _linkAnimator.speed = 1;
                }
            }
            if (_direction.x != 1 && _direction.x != -1)
            {
                _direction.y = Mathf.RoundToInt(Input.GetAxisRaw("Vertical"));
                if (_direction.y != 0)
                {
                    _linkAnimator.SetFloat("Horizontal", 0);

                    _lastDirection.y = _direction.y;
                    _linkAnimator.SetFloat("Vertical", _lastDirection.y);
                    _linkAnimator.speed = 1;
                }
             }
            if (_direction == Vector2.zero)
            {
                _linkAnimator.speed = 0;
            }
        }
        else
        {
            _linkAnimator.speed = 1;
        }
    }

    private void Movement()
    {
        if (!_isAttacking)
        {
            _linkRigidbody.MovePosition(_linkRigidbody.position + _direction * _moveSpeed * Time.fixedDeltaTime);
        }
    }

    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _isAttacking = true;
            _linkAnimator.SetBool("IsAttacking", _isAttacking);
            StartCoroutine(AttackCooldown());
        }
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(0.4f);
        _isAttacking = false;
        _linkAnimator.SetBool("IsAttacking", _isAttacking);
    }
}
