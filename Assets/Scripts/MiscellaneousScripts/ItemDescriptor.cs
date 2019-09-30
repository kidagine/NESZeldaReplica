using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item")]
public class ItemDescriptor : ScriptableObject
{
    public string itemName = "New item";
    public Sprite icon = default;
}
