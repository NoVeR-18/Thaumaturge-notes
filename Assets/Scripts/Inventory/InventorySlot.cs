using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public Button removeButton;
    public Button iconButton;
    public TextMeshProUGUI stackText;

    private Item item;
    private int quantity;
    private void Start()
    {
        removeButton.onClick.AddListener(() => { RemoveItemFromInventory(); });
        iconButton.onClick.AddListener(MoveToHotbar);
    }
    public void AddItem(Item newItem, int count)
    {
        item = newItem;
        quantity = count;

        icon.sprite = item.icon;
        icon.enabled = true;
        removeButton.interactable = true;
        UpdateStackText();
    }

    public void MoveToHotbar()
    {
        if (item != null)
        {
            if (HotbarManager.Instance.AddToHotbar(item, quantity))
            {
                Inventory.instance.Remove(item, quantity);
                ClearSlot();
            }
        }
    }

    public void ClearSlot()
    {
        item = null;
        quantity = 0;

        icon.sprite = null;
        icon.enabled = false;
        removeButton.interactable = false;
        stackText.text = "";
    }

    public void RemoveItemFromInventory()
    {
        if (item != null && quantity > 1)
        {
            quantity--;
            Inventory.instance.DropItem(item);
            UpdateStackText();
        }
        else
        {
            Inventory.instance.DropItem(item);
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
