using System;
using System.Collections;
using UnityEngine;

public class LinkLOZMovement : MonoBehaviour  
{
    [Header("General")]
    [SerializeField] private LinkUI _linkUI = default;
    [SerializeField] private GameObject _shield = default;
    [SerializeField] private ItemSlot _itemSlot = default;
    [SerializeField] private Inventory _invetory = default;
    [SerializeField] private Animator _linkAnimator = default;
    [SerializeField] private GameObject _pfbSwordBeam = default;
    [SerializeField] private Rigidbody2D _linkRigidbody = default;
    [Header("Items")]
    [SerializeField] private GameObject _pfbBomb = default;
    [SerializeField] private GameObject _pfbBoomerang = default;
    [SerializeField] private GameObject _pfbArrow = default;
    private GameObject _swordBeam;
    private GameObject _bomb;
    private GameObject _boomerang;
    private GameObject _arrow;
    private Vector2 _direction;
    private Vector2 _lastDirection;
    private readonly int _moveSpeed = 5;
    private readonly int _knockbackForce = 10;
    private readonly int _heartContainers = 3;
    private int _currentHearts = 6;
    public bool _cantMove;


    public GameObject REMOVETHIS()
    {
        return gameObject;
    }


    void Start()
    {
        _linkUI.HeartSystem.SetHearts(_heartContainers, _currentHearts);
    }

    void Update()
    {
        CheckPlayerDirection();
        Attack();
        UseItem();

        Heal();
    }

    void FixedUpdate()
    {
        Movement();
    }

    private void CheckPlayerDirection()
    {
        if (!_cantMove)
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
        _shield.transform.rotation = GetObjectRotation();
    }

    private void Movement()
    {
        if (!_cantMove)
        {
            _linkRigidbody.MovePosition(_linkRigidbody.position + _direction * _moveSpeed * Time.fixedDeltaTime);
        }
    }

    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!_cantMove)
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
        _swordBeam = Instantiate(_pfbSwordBeam, GetObjectPosition(), GetObjectRotation());
    }

    private void Heal()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (_currentHearts < _heartContainers * 2)
            {
                AudioManager.Instance.Play("LinkDamaged(LOZ)");
                _currentHearts++;
                _linkUI.HeartSystem.SetHearts(_heartContainers, _currentHearts);
            }
        }
    }

    public void Damage(int attackDamage, GameObject attackObject)
    {
        if (attackObject.layer == LayerMask.NameToLayer("Projectile"))
        {
            if (!ShieldDeflected(attackObject))
            {
                InflictDamage(attackDamage, attackObject);
            }
        }
        else
        {
            InflictDamage(attackDamage, attackObject);
        }
    }

    private bool ShieldDeflected(GameObject projectile)
    {
        Vector2 direction = (projectile.transform.position - transform.position).normalized;
        Vector2 strictDirection = new Vector2(Mathf.Round(direction.x * Convert.ToInt32(Mathf.Abs(direction.x) > Mathf.Abs(direction.y))), Mathf.Round(direction.y * Convert.ToInt32(Mathf.Abs(direction.y) > Mathf.Abs(direction.x))));

        if (strictDirection == _lastDirection)
        {
            return true;
        }
        return false;
    }

    private void InflictDamage(int attackDamage, GameObject attackObject)
    {
        Camera.main.GetComponent<LinkPaletteSwap>().StartBlinking();
        if (_invetory.CheckPassiveItem(ItemType.RedRing))
        {
            _currentHearts -= attackDamage / 2;
        }
        else if (_invetory.CheckPassiveItem(ItemType.BlueRing))
        {
            _currentHearts -= attackDamage / 4;
        }
        else
        {
            _currentHearts -= attackDamage;
        }
        AudioManager.Instance.Play("LinkDamaged(LOZ)");
        _linkUI.HeartSystem.SetHearts(_heartContainers, _currentHearts);

        if (_currentHearts <= 0)
        {
            Died();
        }
        else
        {
            KnockBack(attackObject);
        }
    }

    private void KnockBack(GameObject attackObject)
    {
        _cantMove = true;
        Vector2 direction = (transform.position - attackObject.transform.position).normalized;
        Vector2 strictDirection = new Vector2(Mathf.Round(direction.x * Convert.ToInt32(Mathf.Abs(direction.x) > Mathf.Abs(direction.y))), Mathf.Round(direction.y * Convert.ToInt32(Mathf.Abs(direction.y) > Mathf.Abs(direction.x))));
        _linkRigidbody.velocity = strictDirection * _knockbackForce;
        StartCoroutine(ResetVelocity());
    }

    IEnumerator ResetVelocity()
    {
        yield return new WaitForSeconds(0.4f);
        _cantMove = false;
        _linkRigidbody.velocity = Vector2.zero;
    }

    private void Died()
    {
        _cantMove = true;
        _linkRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        AudioManager.Instance.Play("LinkDied(LOZ)");
        _linkAnimator.SetBool("IsDead", true);
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
                    _invetory.UseConsumamble(item);
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
                    case ItemType.Boomerang:
                        ThrowBoomerang();
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
            _arrow = Instantiate(_pfbArrow, GetObjectPosition(), GetObjectRotation());
        }
    }

    private void ThrowBomb()
    {
        if (_bomb == null)
        {
            _linkAnimator.SetBool("IsThrowing", true);
            StartCoroutine(MovementCooldown("IsThrowing", 0.12f));
            _bomb = Instantiate(_pfbBomb, GetObjectPosition(), Quaternion.identity);
        }
    }

    private void ThrowBoomerang()
    {
        if (_boomerang == null)
        {
            _linkAnimator.SetBool("IsThrowing", true);
            StartCoroutine(MovementCooldown("IsThrowing", 0.1f));
            _boomerang = Instantiate(_pfbBoomerang, GetObjectPosition(), GetObjectRotation());
            _boomerang.GetComponent<Boomerang>().SetTarget(gameObject);
        }
    }

    public void CatchBoomerang(GameObject boomerang)
    {
        _linkAnimator.SetBool("IsThrowing", true);
        StartCoroutine(MovementCooldown("IsThrowing", 0.1f));
        Destroy(boomerang);
    }

    private void Push(GameObject obstacle)
    {
        obstacle.GetComponent<Pushable>().Push(gameObject);
    }

    private void AutomaticItemPickUp(GameObject item)
    {
        ItemDescriptor itemDescriptor = item.GetComponent<Item>().GetItemDescriptor();
        if (itemDescriptor.isPassive)
        {
            StartCoroutine(PickupAnimation(item));
        }
        else
        {
            _invetory.Add(itemDescriptor);
            Destroy(item.gameObject);
        }
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
        _cantMove = true;
        yield return new WaitForSeconds(cooldownTime);
        _cantMove = false;
        _linkAnimator.SetBool(animationText, false);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Pushable") && _invetory.CheckPassiveItem(ItemType.PowerBracelet))
        {
            Push(other.gameObject);
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Door"))
        {
            Door door = other.gameObject.GetComponent<Door>();
            if (door.DoorStatus == DoorStatus.Locked)
            {
                if (_linkUI.KeySystem.Keys != 0)
                {
                    _linkUI.ShowPrompt(gameObject);
                    if (Input.GetKeyDown(KeyCode.X))
                    {
                        _linkUI.HidePrompt();
                        _linkUI.KeySystem.UseKey();
                        Interact(other.gameObject);
                    }
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Door"))
        {
            Door door = other.gameObject.GetComponent<Door>();
            if (door.DoorStatus == DoorStatus.Locked)
            {
                _linkUI.HidePrompt();
            }
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
        _invetory.Add(item.GetComponent<Item>().GetItemDescriptor());
        Destroy(item);
        _linkAnimator.SetBool("IsPickingUp", false);
        _linkRigidbody.constraints = RigidbodyConstraints2D.None;
        _linkRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        yield return null;
    }

    IEnumerator PickupAnimation(GameObject item)
    {
        _linkAnimator.SetBool("IsPickingUp", true);
        _linkRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        while (!Input.GetKeyDown(KeyCode.A))
        {
            yield return null;
        }
        _invetory.Add(item.GetComponent<Item>().GetItemDescriptor());
        Destroy(item);
        _linkAnimator.SetBool("IsPickingUp", false);
        _linkRigidbody.constraints = RigidbodyConstraints2D.None;
        _linkRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        yield return null;
    }

    private Vector2 GetObjectPosition()
    {
        Vector2 objectPosition;
        if (_lastDirection.x == 1.0)
            objectPosition = new Vector2(transform.position.x + 1.0f, transform.position.y - 0.06f);
        else if (_lastDirection.x == -1.0)
            objectPosition = new Vector2(transform.position.x - 1.0f, transform.position.y - 0.12f);
        else if (_lastDirection.y == 1.0)
            objectPosition = new Vector2(transform.position.x - 0.12f, transform.position.y + 1.0f);
        else
            objectPosition = new Vector2(transform.position.x + 0.06f, transform.position.y - 1.0f);
        return objectPosition;
    }

    private Quaternion GetObjectRotation()
    {
        Quaternion objectRotation;
        if (_lastDirection.x == 1.0)
            objectRotation = Quaternion.Euler(0, 0, 0);
        else if (_lastDirection.x == -1.0)
            objectRotation = Quaternion.Euler(0, 0, 180);
        else if (_lastDirection.y == 1.0)
            objectRotation = Quaternion.Euler(0, 0, 90);
        else
            objectRotation = Quaternion.Euler(0, 0, 270);
        return objectRotation;
    }
}
