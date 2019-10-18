using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] private Image _itemIcon = default;
    [SerializeField] private Text _itemCounter = default;
    private ItemDescriptor _item;


    public void SetItemSlot(ItemDescriptor item)
    {
        _item = item;
        _itemIcon.sprite = item.icon;
        _itemIcon.enabled = true;
        if (item.isConsumamble)
        {
            _itemCounter.enabled = true;
            _itemCounter.text = item.consumambleUses.ToString();
        }
    }

    public ItemDescriptor GetItemSlot()
    {
        return _item;
    }

    public void EmptyItemSlot()
    {
        _item = null;
        _itemIcon.sprite = null;
        _itemIcon.enabled = false;
        _itemCounter.enabled = false;
    }

    public void IncrementItemSlotCounter(ItemDescriptor item)
    {
        _itemCounter.text = item.consumambleUses.ToString();
    }
}
