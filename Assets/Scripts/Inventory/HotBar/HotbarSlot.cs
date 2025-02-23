using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HotbarSlot : MonoBehaviour
{
    [HideInInspector]
    public int SlotID = 0;

    public Image icon;
    public Sprite defaultIcon;
    public Button iconButton;
    public TextMeshProUGUI stackText;
    public Transform selectionHighlight;

    private Item item;
    private int quantity;

    public bool IsEmpty => item == null;

    private void Start()
    {
        iconButton.onClick.AddListener(() => { RemoveFromHotbar(); HotbarManager.Instance.RemoveFromHotbar(SlotID); });
    }

    public void AddItem(Item newItem, int count = 1)
    {
        item = newItem;
        quantity = count;

        icon.sprite = item.icon;
        icon.enabled = true;
        UpdateStackText();
    }

    public void SetSelected(bool isSelected)
    {
        selectionHighlight.gameObject.SetActive(isSelected);
    }

    public void ClearSlot()
    {
        item = null;
        quantity = 0;
        icon.sprite = defaultIcon;
        stackText.text = "";
    }

    private void RemoveFromHotbar()
    {
        if (item != null)
        {
            Inventory.instance.Add(item, quantity); // Возвращаем предмет в инвентарь
            ClearSlot();
        }
    }

    private void UpdateStackText()
    {
        stackText.text = quantity > 1 ? quantity.ToString() : "";
    }
}
