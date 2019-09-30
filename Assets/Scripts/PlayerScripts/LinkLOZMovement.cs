using UnityEngine;
using System.Collections;

public class LinkLOZMovement : MonoBehaviour  
{
    [SerializeField] private HeartSystem _playerUI = default;
    [SerializeField] private Animator _linkAnimator = default;
    [SerializeField] private GameObject _pfbSwordBeam = default;
    [SerializeField] private Rigidbody2D _linkRigidbody = default;
    private GameObject swordBeam;
    private Vector2 _direction;
    private Vector2 _lastDirection;
    private int _moveSpeed = 5;
    private int _currentHearts = 6;
    private int _heartContainers = 3;
    private bool _isPositionLocked;
    private bool _isAttacking;


    private void Start()
    {
        _playerUI.SetHearts(_heartContainers, _currentHearts);   
    }

    void Update()
    {
        CheckPlayerDirection();
        Attack();
        Damaged();
        Heal();
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
                if (_currentHearts == _heartContainers * 2 && swordBeam == null)
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

    private void Heal()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (_currentHearts < _heartContainers * 2)
            {
                AudioManager.Instance.Play("LinkDamaged(LOZ)");
                _currentHearts++;
                _playerUI.SetHearts(_heartContainers, _currentHearts);
                if (_currentHearts <= 0)
                {
                    Died();
                }
            }

        }
    }

    private void Damaged()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            AudioManager.Instance.Play("LinkDamaged(LOZ)");
            _currentHearts--;
            _playerUI.SetHearts(_heartContainers, _currentHearts);
            if (_currentHearts <= 0)
            {
                Died();
            }
        }
    }

    private void Died()
    {
        AudioManager.Instance.Play("LinkDied(LOZ)");
        Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Interactable"))
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                IInteractable interactable = other.gameObject.GetComponent<IInteractable>();
                interactable.Interact();
                InteractableType interactableType = interactable.GetInteractableType();
                Interact(interactableType, interactable);
            }
        }
    }

    private void Interact(InteractableType interactableType, IInteractable interactable)
    {
        switch (interactableType)
        {
            case InteractableType.Chest:
                StartCoroutine(OpenChest(interactable));
                break;
            default:
                break;
        }
    }

    IEnumerator OpenChest(IInteractable interactable)
    {
        GameObject chestObject = interactable.getObject();
        Chest chest = chestObject.GetComponent<Chest>();
        GameObject item = chest.GetItem();
        _linkAnimator.SetBool("IsPickingUp", true);
        _linkRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        while (!Input.GetKeyDown(KeyCode.A))
        {
            yield return null;
        }
        Inventory.Instance.Add(item.GetComponent<Item>().GetItemDescriptor());
        Destroy(item);
        _linkAnimator.SetBool("IsPickingUp", false);
        _linkRigidbody.constraints = RigidbodyConstraints2D.None;
        _linkRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        yield return null;
    }
}
