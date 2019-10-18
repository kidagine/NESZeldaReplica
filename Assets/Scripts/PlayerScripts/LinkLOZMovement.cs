using System;
using System.Collections;
using UnityEngine;

public class LinkLOZMovement : MonoBehaviour  
{
    [Header("General")]
    [SerializeField] private ItemSlot _itemSlot = default;
    [SerializeField] private HeartSystem _playerUI = default;
    [SerializeField] private Animator _linkAnimator = default;
    [SerializeField] private GameObject _pfbSwordBeam = default;
    [SerializeField] private Rigidbody2D _linkRigidbody = default;
    [Header("Items")]
    [SerializeField] private GameObject _pfbBomb = default;
    [SerializeField] private GameObject _pfbArrow = default;
    private GameObject _swordBeam;
    private GameObject _bomb;
    private GameObject _arrow;
    private Vector2 _direction;
    private Vector2 _lastDirection;
    private int _moveSpeed = 5;
    private int _currentHearts = 6;
    private int _heartContainers = 3;
    private bool _isPositionLocked;
    private bool _canMove;


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
        UseItem();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void CheckPlayerDirection()
    {
        if (!_canMove)
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
        if (!_canMove)
        {
            _linkRigidbody.MovePosition(_linkRigidbody.position + _direction * _moveSpeed * Time.fixedDeltaTime);
        }
    }

    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!_canMove)
            {
                AudioManager.Instance.Play("SwordSlash(LOZ)");
                if (_currentHearts == _heartContainers * 2 && _swordBeam == null)
                {
                    StartCoroutine(FireBeam());
                }
                _linkAnimator.SetBool("IsAttacking", true);
                StartCoroutine(MovementCooldown("IsAttacking", 0.5f));
            }
        }
    }

    IEnumerator FireBeam()
    {
        yield return new WaitForSeconds(0.3f);
        AudioManager.Instance.Play("SwordBeam(LOZ)");
        _swordBeam = Instantiate(_pfbSwordBeam, GetPrefabPosition(), GetPrefabRotation());
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

    private void UseItem()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            ItemDescriptor item = _itemSlot.GetItemSlot();
            if (item != null)
            {
                if (item.isConsumamble)
                {
                    Inventory.Instance.UseConsumamble(item);
                }

                ItemType itemType = item.itemType;
                switch (itemType)
                {
                    case ItemType.Bow:
                        FireBow();
                        break;
                    case ItemType.Bomb:
                        ThrowBomb();
                        break;
                }
            }
        }
    }

    private void FireBow()
    {
        if (_arrow == null)
        {
            _linkAnimator.SetBool("IsThrowing", true);
            StartCoroutine(MovementCooldown("IsThrowing", 0.1f));
            _arrow = Instantiate(_pfbArrow, GetPrefabPosition(), GetPrefabRotation());
        }
    }

    private void ThrowBomb()
    {
        if (_bomb == null)
        {
            _linkAnimator.SetBool("IsThrowing", true);
            StartCoroutine(MovementCooldown("IsThrowing", 0.12f));
            _bomb = Instantiate(_pfbBomb, GetPrefabPosition(), Quaternion.identity);
        }
    }

    private void Push(GameObject obstacle)
    {
        obstacle.GetComponent<Pushable>().Push(gameObject);
    }

    private void AutomaticItemPickUp(GameObject item)
    {
        Inventory.Instance.Add(item.GetComponent<Item>().GetItemDescriptor());
        Destroy(item.gameObject);
    }

    private void Interact(GameObject interactableObject)
    {
        IInteractable interactable = interactableObject.GetComponent<IInteractable>();
        interactable.Interact();
        InteractableType interactableType = interactable.GetInteractableType();
        Interact(interactableType, interactable);
    }

    IEnumerator MovementCooldown(string animationText, float cooldownTime)
    {
        _canMove = true;
        yield return new WaitForSeconds(cooldownTime);
        _canMove = false;
        _linkAnimator.SetBool(animationText, false);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Pushable") && Inventory.Instance.CheckPassiveItem(ItemType.PowerBracelet))
        {
            Push(other.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Pickable"))
        {
            AutomaticItemPickUp(other.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Interactable"))
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                Interact(other.gameObject);
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

    private Vector2 GetPrefabPosition()
    {
        Vector2 prefabPosition;
        if (_lastDirection.x == 1.0)
            prefabPosition = new Vector2(transform.position.x + 1.0f, transform.position.y - 0.06f);
        else if (_lastDirection.x == -1.0)
            prefabPosition = new Vector2(transform.position.x - 1.0f, transform.position.y - 0.12f);
        else if (_lastDirection.y == 1.0)
            prefabPosition = new Vector2(transform.position.x - 0.12f, transform.position.y + 1.0f);
        else
            prefabPosition = new Vector2(transform.position.x + 0.06f, transform.position.y - 1.0f);
        return prefabPosition;
    }

    private Quaternion GetPrefabRotation()
    {
        Quaternion prefabRotation;
        if (_lastDirection.x == 1.0)
            prefabRotation = Quaternion.Euler(0, 0, 0);
        else if (_lastDirection.x == -1.0)
            prefabRotation = Quaternion.Euler(0, 0, 180);
        else if (_lastDirection.y == 1.0)
            prefabRotation = Quaternion.Euler(0, 0, 90);
        else
            prefabRotation = Quaternion.Euler(0, 0, 270);
        return prefabRotation;
    }
}
