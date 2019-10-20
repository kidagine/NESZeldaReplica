using UnityEngine;

public enum DoorStatus { Open, Closed, Locked }

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private DoorStatus _doorStatus = default;
    [SerializeField] private Sprite _openDoor = default;
    [SerializeField] private Sprite _closedDoor = default;
    [SerializeField] private Sprite _lockedDoor = default;
    [SerializeField] private SpriteRenderer _door = default;
    [SerializeField] private BoxCollider2D _doorCollider = default;
    public DoorStatus DoorStatus { get { return _doorStatus; } private set { _doorStatus = value; } }

    public InteractableType GetInteractableType()
    {
        return InteractableType.Door;
    }

    public GameObject getObject()
    {
        return gameObject;
    }

    public void Interact()
    {
        OpenDoor();
    }

    public void OpenDoor()
    {
        _door.sprite = _openDoor;
        _doorCollider.enabled = false;
    }
    
}
