public enum InteractableType { Chest }

public interface IInteractable
{
    void Interact();
    InteractableType GetInteractableType();
}
