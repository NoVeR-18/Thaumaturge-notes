using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [HideInInspector]
    public int SlotID = 0;
    public Image icon;
    public Sprite defaultIcon;
    public Button removeButton;
    public Button iconButton;
    public TextMeshProUGUI stackText;

    public Item item;
    public int quantity;
    private void Start()
    {
        removeButton.onClick.AddListener(() => { RemoveItemFromInventory(); });
        iconButton.onClick.AddListener(() => { Inventory.Instance.OnSlotClicked(SlotID); });
    }
    public void AddItem(Item newItem, int count)
    {
        item = newItem;
        quantity = count;

        icon.sprite = item.icon;
        removeButton.interactable = true;
        UpdateStackText();
    }
    public void AddItem(InventoryItem newItem)
    {
        if (newItem != null)
        {
            if (newItem.item != null)
            {
                item = newItem.item;
                quantity = newItem.quantity;
                icon.sprite = item.icon;
                removeButton.interactable = true;
                UpdateStackText();
            }
            else
            {
                ClearSlot();
            }
        }
        else
        {
            ClearSlot();
        }
    }

    public void MoveToHotbar()
    {
        if (item != null)
        {
            if (HotbarManager.Instance.AddToHotbar(item, quantity))
            {
                Inventory.Instance.Remove(item, quantity);
                ClearSlot();
            }
        }
    }

    public void ClearSlot()
    {
        item = null;
        quantity = 0;

        icon.sprite = defaultIcon;
        removeButton.interactable = false;

        UpdateStackText();
    }

    public void RemoveItemFromInventory()
    {
        if (item != null && quantity > 1)
        {
            quantity--;
            Inventory.Instance.DropItem(item);
            UpdateStackText();
        }
        else
        {
            Inventory.Instance.DropItem(item);
            ClearSlot();
        }
    }

    public void UseItem()
    {
        if (item != null)
        {
            item.Use();
            RemoveItemFromInventory();
        }
    }

    private void UpdateStackText()
    {
        stackText.text = quantity > 1 ? quantity.ToString() : "";
    }
}
