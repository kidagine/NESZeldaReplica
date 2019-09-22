using UnityEngine;
using System.Collections;

public class LinkLOZMovement : MonoBehaviour
{
    [SerializeField] private Animator _linkAnimator = default;
    [SerializeField] private GameObject _pfbSwordBeam = default;
    [SerializeField] private Rigidbody2D _linkRigidbody = default;
    private GameObject swordBeam;
    private Vector2 _direction;
    private Vector2 _lastDirection;
    private readonly int _moveSpeed = 5;
    private int _maxHearts = 3;
    private int _currentHearts = 3;
    private bool _isAttacking;


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

                    _lastDirection.y = 0;
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

                    _lastDirection.x = 0;
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
            if (!_isAttacking)
            {
                AudioManager.Instance.Play("SwordSlash(LOZ)");
                if (_currentHearts == _maxHearts && swordBeam == null)
                {
                    Invoke("FireBeam", 0.28f);
                }
                _isAttacking = true;
                _linkAnimator.SetBool("IsAttacking", _isAttacking);
                StartCoroutine(AttackCooldown());
            }
        }
    }

    private void FireBeam()
    {
        AudioManager.Instance.Play("SwordBeam(LOZ)");
        Vector2 swordBeamPosition;
        Quaternion swordBeamRotation;
        if (_lastDirection.x == 1.0)
        {
            swordBeamPosition = new Vector2(transform.position.x + 1.0f, transform.position.y - 0.06f);
            swordBeamRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (_lastDirection.x == -1.0)
        {
            swordBeamPosition = new Vector2(transform.position.x - 1.0f, transform.position.y - 0.12f);
            swordBeamRotation = Quaternion.Euler(0, 0, 180);
        }
        else if (_lastDirection.y == 1.0)
        {
            swordBeamPosition = new Vector2(transform.position.x - 0.12f, transform.position.y + 1.0f);
            swordBeamRotation = Quaternion.Euler(0, 0, 90);
        }
        else
        {
            swordBeamPosition = new Vector2(transform.position.x + 0.06f, transform.position.y - 1.0f);
            swordBeamRotation = Quaternion.Euler(0, 0, 270);
        }
        swordBeam = Instantiate(_pfbSwordBeam, swordBeamPosition, swordBeamRotation);
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(0.5f);
        _isAttacking = false;
        _linkAnimator.SetBool("IsAttacking", _isAttacking);
    }
}
