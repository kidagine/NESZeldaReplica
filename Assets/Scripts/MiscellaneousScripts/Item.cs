using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private ItemDescriptor itemDescriptor;

    public ItemDescriptor GetItemDescriptor()
    {
        return itemDescriptor;
    }
}
