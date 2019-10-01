using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private ItemDescriptor _itemDescriptor = default;

    public ItemDescriptor GetItemDescriptor()
    {
        return _itemDescriptor;
    }
}
