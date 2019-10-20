using UnityEngine;

public enum ItemType { Bow, Bomb, PowerBracelet, Key }

[CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item")]
public class ItemDescriptor : ScriptableObject
{
    public string itemName = "New item";
    public ItemType itemType = default;
    public bool isConsumamble = default;
    public bool isPassive = default;
    public int consumambleUses = default;
    public Sprite icon = default;
}
