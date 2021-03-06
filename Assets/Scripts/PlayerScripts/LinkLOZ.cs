﻿using System;
using System.Collections;
using UnityEngine;

public class LinkLOZ : MonoBehaviour  
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
    [SerializeField] private GameObject _pfbArrow = default;
    [SerializeField] private GameObject _pfbBoomerang = default;
    private GameObject _bomb;
    private GameObject _arrow;
    private GameObject _boomerang;
    private GameObject _swordBeam;
    private Vector2 _direction;
    private Vector2 _lastDirection;
    private readonly int _moveSpeed = 5;
    private readonly int _maxKeysAmount = 5;
    private readonly int _knockbackForce = 10;
    private int _currentKeys;
    private int _currentHearts = 6;
    private int _heartContainers = 3;


    public bool CantMove { private get; set; }

    void Start()
    {
        _linkUI.HeartSystem.SetHearts(_heartContainers, _currentHearts);
    }

    void Update()
    {
        CheckPlayerDirection();
        Attack();
        UseItem();
    }

    void FixedUpdate()
    {
        Movement();
    }

    private void CheckPlayerDirection()
    {
        if (!CantMove)
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
        if (!CantMove)
        {
            _linkRigidbody.MovePosition(_linkRigidbody.position + _direction * _moveSpeed * Time.fixedDeltaTime);
        }
    }

    private void Attack()
    {
        if (Input.GetButtonDown("B"))
        {
            if (!CantMove)
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
        AudioManager.Instance.Play("ShieldDeflect(LOZ)");
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

        Camera.main.GetComponent<LinkPaletteSwap>().StartBlinking(gameObject);
        SetInvisibility(true);

        if (_currentHearts <= 0)
        {
            Died();
        }
        else
        {
            KnockBack(attackObject);
        }
    }

    public void SetInvisibility(bool isInvisible)
    {
        if (isInvisible)
        {
            Physics2D.IgnoreLayerCollision(gameObject.layer, gameObject.layer, true);
        }
        else
        {
            Physics2D.IgnoreLayerCollision(gameObject.layer, gameObject.layer, false);
        }
    }

    private void KnockBack(GameObject attackObject)
    {
        CantMove = true;
        Vector2 direction = (transform.position - attackObject.transform.position).normalized;
        Vector2 strictDirection = new Vector2(Mathf.Round(direction.x * Convert.ToInt32(Mathf.Abs(direction.x) > Mathf.Abs(direction.y))), Mathf.Round(direction.y * Convert.ToInt32(Mathf.Abs(direction.y) > Mathf.Abs(direction.x))));
        _linkRigidbody.velocity = strictDirection * _knockbackForce;
        StartCoroutine(ResetVelocity());
    }

    IEnumerator ResetVelocity()
    {
        yield return new WaitForSeconds(0.1f);
        CantMove = false;
        _linkRigidbody.velocity = Vector2.zero;
        if (_currentHearts <= 2)
        {
            AudioManager.Instance.Play("LinkLowHealth(LOZ)");
        }
    }

    private void Died()
    {
        AudioManager.Instance.Play("LinkDied(LOZ)");
        CantMove = true;
        _linkRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        _linkAnimator.SetBool("IsDead", true);
    }

    private void UseItem()
    {
        if (Input.GetButtonDown("X") || Input.GetButtonDown("Y"))
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
            AudioManager.Instance.Play("Arrow(LOZ)");
            _linkAnimator.SetBool("IsThrowing", true);
            StartCoroutine(MovementCooldown("IsThrowing", 0.1f));
            _arrow = Instantiate(_pfbArrow, GetObjectPosition(), GetObjectRotation());
        }
    }

    private void ThrowBomb()
    {
        if (_bomb == null)
        {
            AudioManager.Instance.Play("BombThrow(LOZ)");
            _linkAnimator.SetBool("IsThrowing", true);
            StartCoroutine(MovementCooldown("IsThrowing", 0.12f));
            _bomb = Instantiate(_pfbBomb, GetObjectPosition(), Quaternion.identity);
        }
    }

    private void ThrowBoomerang()
    {
        if (_boomerang == null)
        {
            AudioManager.Instance.Play("Arrow-Boomerang(LOZ)");
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Pickable"))
        {
            ItemDescriptor itemDescriptor = other.gameObject.GetComponent<Item>().GetItemDescriptor();
            if (itemDescriptor.isImportant)
            {
                StartCoroutine(HoldUpItem(other.gameObject));
            }
            else
            {
                AutomaticItemPickUp(other.gameObject);
            }
        }
    }

    IEnumerator HoldUpItem(GameObject item)
    {
        AudioManager.Instance.Play("PickupJingle(LOZ)");
        AudioManager.Instance.PauseEverythingExpect("PickupJingle(LOZ)");
        Time.timeScale = 0.0f;
        item.transform.position = new Vector2(transform.position.x, transform.position.y + 0.75f);

        _linkAnimator.SetBool("IsPickingUp", true);
        _linkRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        while (!Input.GetButtonDown("A"))
        {
            yield return null;
        }
        AudioManager.Instance.ResumeEverything();
        Time.timeScale = 1.0f;

        _linkAnimator.SetBool("IsPickingUp", false);
        _linkRigidbody.constraints = RigidbodyConstraints2D.None;
        _linkRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        AutomaticItemPickUp(item);
    }

    private void AutomaticItemPickUp(GameObject item)
    {
        ItemDescriptor itemDescriptor = item.GetComponent<Item>().GetItemDescriptor();
        if (itemDescriptor.hasImmidieteEffect)
        {
            switch (itemDescriptor.itemType)
            {
                case ItemType.Heart:
                    AudioManager.Instance.Play("PickupHeart(LOZ)");
                    Heal();
                    break;
                case ItemType.HeartContainer:
                    SetHeartContainers();
                    break;
                case ItemType.Rupee:
                    AudioManager.Instance.Play("PickupRupee(LOZ)");
                    _linkUI.RupeeSystem.SetRupees(1);
                    break;
                case ItemType.Key:
                    AudioManager.Instance.Play("PickupItem(LOZ)");
                    GetKey();
                    break;
            }
        }
        else
        {
            _invetory.Add(itemDescriptor);
        }
        Destroy(item);
    }

    private void Heal()
    {
        if (_currentHearts < _heartContainers * 2)
        {
            if (_currentHearts % 2 == 0)
            {
                _currentHearts += 2;
            }
            else
            {
                _currentHearts++;
            }
            _linkUI.HeartSystem.SetHearts(_heartContainers, _currentHearts);
        }
        if (AudioManager.Instance.IsPlaying("LinkLowHealth(LOZ)"))
        {
            if (_currentHearts >= 2)
            {
                AudioManager.Instance.Stop("LinkLowHealth(LOZ)");
            }
        }
    }

    public void SetHeartContainers()
    {
        _heartContainers++;
        _linkUI.HeartSystem.SetHearts(_heartContainers, _currentHearts);
    }

    private void GetKey()
    {
        if (_currentKeys < _maxKeysAmount)
        {
            _currentKeys++;
            _linkUI.KeySystem.SetKeys(_currentKeys);
        }
    }

    IEnumerator MovementCooldown(string animationText, float cooldownTime)
    {
        CantMove = true;
        yield return new WaitForSeconds(cooldownTime);
        CantMove = false;
        _linkAnimator.SetBool(animationText, false);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Pushable") && _invetory.CheckPassiveItem(ItemType.PowerBracelet))
        {
            Push(other.gameObject);
        }
    }

    private void Interact(GameObject interactableObject)
    {
        IInteractable interactable = interactableObject.GetComponent<IInteractable>();
        interactable.Interact();
        InteractableType interactableType = interactable.GetInteractableType();
        Interact(interactableType, interactable);
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
                    if (Input.GetButtonDown("A"))
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

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Interactable"))
        {
            if (Input.GetButtonDown("A"))
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
        GameObject chestObject = interactable.GetObject();
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
