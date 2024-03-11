using TMPro;
using UnityEngine;

public class InventorySlotView : MonoBehaviour
{
    [SerializeField] private InventoryItemsTypes inventoryItemViewed;
    [SerializeField] private TextMeshProUGUI count;
    [SerializeField] private Sprite icon;

    public void UpdateCount(int count)
    {
        this.count.text = count.ToString();
    }
}