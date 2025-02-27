using Assets.Scripts.UI;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryWindow : WindowBase
{
    [SerializeField]
    private List<InventorySlot> slots;
    [SerializeField]
    private GameObject SlotUIPrefab;
    [SerializeField]
    private Transform SlotContainer;

    private InventorySlot selectedSlot;

    private int itemCount;

    private void Awake()
    {
        itemCount = Inventory.Instance.space;
        for (int i = 0; i < itemCount; i++)
        {
            var slot = Instantiate(SlotUIPrefab, SlotContainer).GetComponent<InventorySlot>();
            slot.icon.sprite = null;
            slot.SlotID = i;
            slots.Add(slot);
        }
        if (selectedSlot == null)
            selectedSlot = Instantiate(SlotUIPrefab, transform).GetComponent<InventorySlot>();
        selectedSlot.AddComponent<CanvasGroup>().blocksRaycasts = false;
        selectedSlot.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        Inventory.Instance.InventoryUpdated += UpdateUI;
    }

    private void OnDisable()
    {
        Inventory.Instance.InventoryUpdated -= UpdateUI;
    }

    public override void OpenTab()
    {
        base.OpenTab();
        UpdateUI();
    }

    private void Update()
    {
        if (selectedSlot.item != null && selectedSlot.gameObject.activeSelf)
        {
            selectedSlot.transform.position = Input.mousePosition;
        }
    }

    public void SetSelectedSlot(InventoryItem selectedItem)
    {
        selectedSlot.gameObject.SetActive(true);
        selectedSlot.AddItem(selectedItem);
    }

    public void ClearSelectedSlot()
    {
        if (selectedSlot.item != null)
        {
            selectedSlot.item = new Item();
            selectedSlot.gameObject.SetActive(false);
        }
    }


    public override void CloseTab()
    {
        base.CloseTab();
        if (Inventory.Instance.SelectedItem.item != null)
        {
            Inventory.Instance.Add(Inventory.Instance.SelectedItem);
            Inventory.Instance.ClearSelectedItem();
        }
        ClearSelectedSlot();
    }

    public void UpdateUI()
    {
        var items = Inventory.Instance.items;
        for (int i = 0; i < slots.Count; i++)
        {
            if (items.Count > i)
            {
                slots[i].AddItem(items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }
}