using Assets.Scripts.UI;
using System.Collections.Generic;
using UnityEngine;

public class InventoryWindow : WindowBase
{
    [SerializeField]
    private List<InventorySlot> slots;
    [SerializeField]
    private GameObject SlotUIPrefab;

    int itemCount;

    private void Awake()
    {
        itemCount = Inventory.instance.space;
        for (int i = 0; i < itemCount; i++)
        {
            var slot = Instantiate(SlotUIPrefab, gameObject.transform);
            slots.Add(slot.GetComponent<InventorySlot>());
        }

    }

    private void OnEnable()
    {
        Inventory.instance.InventoryUpdated += UpdateUI;
    }

    private void OnDisable()
    {
        Inventory.instance.InventoryUpdated -= UpdateUI;
    }

    public override void OpenTab()
    {
        base.OpenTab();
        UpdateUI();
    }

    public void UpdateUI()
    {
        var items = Inventory.instance.items;
        for (int i = 0; i < slots.Count; i++)
        {
            if (items.Count > i)
            {
                slots[i].AddItem(items[i].item, Inventory.instance.GetItemCount(items[i]));
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }
}