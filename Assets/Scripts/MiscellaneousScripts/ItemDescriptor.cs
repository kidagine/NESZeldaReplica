using UnityEngine;

public enum ItemType { Bow, Bomb }

[CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item")]
public class ItemDescriptor : ScriptableObject
{
    public string itemName = "New item";
    public ItemType itemType = default;
    public Sprite icon = default;
}
