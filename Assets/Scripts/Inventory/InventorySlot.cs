using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public Button removeButton;
    public TextMeshProUGUI stackText;

    private Item item;
    private int quantity;
    private void Start()
    {
        removeButton.onClick.AddListener(() => { RemoveItemFromInventory(); });


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
