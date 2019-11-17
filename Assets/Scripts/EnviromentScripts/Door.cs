using UnityEngine;

public enum DoorStatus { Open, Closed, Locked }
public enum DoorPosition { Top, Bottom, Left, Right }

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private DoorStatus _doorStatus = default;
    [SerializeField] private Sprite _openDoor = default;
    [SerializeField] private Sprite _closedDoor = default;
    [SerializeField] private SpriteRenderer _door = default;
    [SerializeField] private BoxCollider2D _doorCollider = default;
    private GameObject _connectedDoor;

    public DoorStatus DoorStatus { get { return _doorStatus; } private set { _doorStatus = value; } }
    public DoorPosition DoorPosition { get; private set; }


    void Start()
    {
        if (_doorStatus == DoorStatus.Open)
        {
            _doorCollider.enabled = false;
        }
        GetDoorPosition();
    }

    private void GetDoorPosition()
    {
        if (transform.rotation == Quaternion.Euler(0, 0, 0))
        {
            DoorPosition = DoorPosition.Top;
        }
        else if (transform.rotation == Quaternion.Euler(0, 0, 180))
        {
            DoorPosition = DoorPosition.Bottom;
        }
        else if (transform.rotation == Quaternion.Euler(0, 0, 90))
        {
            DoorPosition = DoorPosition.Left;
        }
        else
        {
            DoorPosition = DoorPosition.Right;
        }
    }

    public InteractableType GetInteractableType()
    {
        return InteractableType.Door;
    }

    public GameObject GetObject()
    {
        return gameObject;
    }

    public void SetConnectedDoor(GameObject connectedDoor)
    {
        _connectedDoor = connectedDoor;
    }

    public void Interact()
    {
        OpenDoor();
    }

    public void OpenDoor()
    {
        if (_doorStatus != DoorStatus.Open)
        {
            _door.sprite = _openDoor;
            _doorCollider.enabled = false;
            _doorStatus = DoorStatus.Open;
            if (_connectedDoor != null)
            {
                _connectedDoor.GetComponent<Door>().OpenDoor();
            }
        }
    }
}
