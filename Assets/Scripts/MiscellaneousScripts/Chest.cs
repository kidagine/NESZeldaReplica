using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    [SerializeField] private Sprite _openedChest = default;
    [SerializeField] private SpriteRenderer chestSpriteRenderer = default;


    public void Interact()
    {
        OpenChest();
    }

    private void OpenChest()
    {
        chestSpriteRenderer.sprite = _openedChest;
        GameObject item = gameObject.transform.GetChild(0).gameObject;
        item.SetActive(true);
    }

    public GameObject GetItem()
    {
        return gameObject.transform.GetChild(0).gameObject;
    }

    public InteractableType GetInteractableType()
    {
        return InteractableType.Chest;
    }

    public GameObject getObject()
    {
        return gameObject;
    }
}
