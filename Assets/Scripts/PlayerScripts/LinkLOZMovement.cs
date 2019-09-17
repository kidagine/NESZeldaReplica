using UnityEngine;

public class LinkLOZMovement : MonoBehaviour
{
    [SerializeField] private Animator _playerAnimator = default;
    [SerializeField] private Rigidbody2D _playerRigidbody = default;
    private Vector2 _direction;
    private Vector2 _lastDirection;
    private readonly int _moveSpeed = 3;


    void Update()
    {
        CheckPlayerDirection();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void CheckPlayerDirection()
    {
        if (_direction.y != 1 && _direction.y != -1)
        {
            _direction.x = Mathf.RoundToInt(Input.GetAxisRaw("Horizontal"));
            if (_direction.x != 0)
            {
                _playerAnimator.SetFloat("Vertical", 0);

                _lastDirection.x = _direction.x;
                _playerAnimator.SetFloat("Horizontal", _lastDirection.x);
                _playerAnimator.speed = 1;
            }
        }
        if (_direction.x != 1 && _direction.x != -1)
        {
            _direction.y = Mathf.RoundToInt(Input.GetAxisRaw("Vertical"));
            if (_direction.y != 0)
            {
                _playerAnimator.SetFloat("Horizontal", 0);

                _lastDirection.y = _direction.y;
                _playerAnimator.SetFloat("Vertical", _lastDirection.y);
                _playerAnimator.speed = 1;
            }
        }
        if (_direction == Vector2.zero)
        {
            _playerAnimator.speed = 0;
        }
    }

    private void Movement()
    {
        _playerRigidbody.MovePosition(_playerRigidbody.position + _direction * _moveSpeed * Time.fixedDeltaTime);
    }
}
