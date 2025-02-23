using System.Collections.Generic;
using UnityEngine;

public class HotbarWindow : MonoBehaviour
{
    [SerializeField] private List<HotbarSlot> slots;
    [SerializeField] private GameObject slotPrefab;

    private HotbarManager hotbarManager;

    private void Start()
    {
        hotbarManager = HotbarManager.Instance;
        for (int i = 0; i < hotbarManager.hotbarSize; i++)
        {
            var slot = Instantiate(slotPrefab, transform).GetComponent<HotbarSlot>();
            slot.icon.sprite = null;
            slot.SlotID = i;
            slots.Add(slot);
        }

        hotbarManager.HotbarUpdated += UpdateUI;
        UpdateUI();
    }

    private void UpdateUI()
    {
        var items = hotbarManager.GetItems();
        for (int i = 0; i < slots.Count; i++)
        {
            if (items[i] != null)
            {
                if (items[i].item != null)
                {
                    slots[i].AddItem(items[i].item, items[i].quantity);
                }
                else slots[i].ClearSlot();
            }
            else slots[i].ClearSlot();
            slots[i].SetSelected(i == hotbarManager.SelectedSlotIndex);
        }
    }
}
