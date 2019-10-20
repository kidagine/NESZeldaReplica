using UnityEngine;

public enum InteractableType { Chest, Door }

public interface IInteractable
{
    void Interact();
    InteractableType GetInteractableType();
    GameObject getObject();
}
