using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public Item item;
    public int quantity;

    public InventoryItem()
    {
        item = null;
        quantity = 0;
    }

    public InventoryItem(Item newItem, int amount)
    {
        item = newItem;
        quantity = amount;
    }

    public bool IsEmpty()
    {
        return item == null || quantity <= 0;
    }

    public void Clear()
    {
        item = null;
        quantity = 0;
    }
}

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    public int space = 10; // Количество слотов
    public List<InventoryItem> items = new List<InventoryItem>();
    public Transform dropPosition; // Точка выброса предмета
    public InventoryItem SelectedItem = null;
    public Action InventoryUpdated;

    private InventoryWindow inventoryWindow;

    void Awake()
    {
        Instance = this;
        InitializeInventory();
        LoadInventory();
    }

    private void Start()
    {
        inventoryWindow = WindowsManager.instance._inventoryPopup;
    }

    private void InitializeInventory()
    {
        items = new List<InventoryItem>();
        for (int i = 0; i < space; i++)
        {
            items.Add(new InventoryItem()); // Заполняем пустыми слотами
        }
    }

    public void Add(InventoryItem item)
    {
        int remainingAmount = item.quantity;

        // Заполняем существующие слоты
        foreach (var inventoryItem in items)
        {
            if (inventoryItem.item == item.item && inventoryItem.quantity < item.item.maxStack)
            {
                int addable = Mathf.Min(item.item.maxStack - inventoryItem.quantity, remainingAmount);
                inventoryItem.quantity += addable;
                remainingAmount -= addable;

                if (remainingAmount <= 0)
                {
                    InventoryUpdated?.Invoke();
                    return;
                }
            }
        }

        // Записываем в пустые слоты
        foreach (var inventoryItem in items)
        {
            if (inventoryItem.IsEmpty())
            {
                int stackSize = Mathf.Min(remainingAmount, item.item.maxStack);
                inventoryItem.item = item.item;
                inventoryItem.quantity = stackSize;
                remainingAmount -= stackSize;

                if (remainingAmount <= 0)
                {
                    InventoryUpdated?.Invoke();
                    return;
                }
            }
        }

        if (remainingAmount > 0)
        {
            Debug.Log("Инвентарь переполнен!");
        }

        InventoryUpdated?.Invoke();
        SaveInventory();
    }
    public void Add(Item item, int amount = 1)
    {
        int remainingAmount = amount;

        // Заполняем существующие слоты
        foreach (var inventoryItem in items)
        {
            if (inventoryItem.item == item && inventoryItem.quantity < item.maxStack)
            {
                int addable = Mathf.Min(item.maxStack - inventoryItem.quantity, remainingAmount);
                inventoryItem.quantity += addable;
                remainingAmount -= addable;

                if (remainingAmount <= 0)
                {
                    InventoryUpdated?.Invoke();
                    return;
                }
            }
        }

        // Записываем в пустые слоты
        foreach (var inventoryItem in items)
        {
            if (inventoryItem.IsEmpty())
            {
                int stackSize = Mathf.Min(remainingAmount, item.maxStack);
                inventoryItem.item = item;
                inventoryItem.quantity = stackSize;
                remainingAmount -= stackSize;

                if (remainingAmount <= 0)
                {
                    InventoryUpdated?.Invoke();
                    return;
                }
            }
        }

        if (remainingAmount > 0)
        {
            Debug.Log("Инвентарь переполнен!");
        }

        InventoryUpdated?.Invoke();
        SaveInventory();
    }

    public void Remove(Item item, int amount = 1)
    {
        foreach (var inventoryItem in items)
        {
            if (inventoryItem.item == item)
            {
                inventoryItem.quantity -= amount;
                if (inventoryItem.quantity <= 0)
                {
                    inventoryItem.Clear();
                }
                break;
            }
        }

        InventoryUpdated?.Invoke();
        SaveInventory();
    }

    public void DropItem(Item item)
    {
        foreach (var inventoryItem in items)
        {
            if (inventoryItem.item == item)
            {
                GameObject droppedItem = Instantiate(item.prefab, dropPosition.position, Quaternion.identity);
                Remove(item);
                break;
            }
        }
    }

    public int GetItemCount(Item item)
    {
        int count = 0;
        foreach (var inventoryItem in items)
        {
            if (inventoryItem.item == item)
            {
                count += inventoryItem.quantity;
            }
        }
        return count;
    }

    public void SaveInventory()
    {
        InventoryData data = new InventoryData(items);
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("Inventory", json);
        PlayerPrefs.Save();
    }

    public void LoadInventory()
    {
        if (PlayerPrefs.HasKey("Inventory"))
        {
            string json = PlayerPrefs.GetString("Inventory");
            InventoryData data = JsonUtility.FromJson<InventoryData>(json);
            items = data.items ?? new List<InventoryItem>();

            // Если загруженный инвентарь меньше нужного размера, дополняем пустыми слотами
            while (items.Count < space)
            {
                items.Add(new InventoryItem());
            }
        }
    }

    public void OnSlotClicked(int slotIndex)
    {
        var selectedItem = SelectedItem;

        if (selectedItem.item == null) // Если ничего не выбрано
        {
            if (items[slotIndex].item != null) // Если в слоте есть предмет
            {
                selectedItem = new InventoryItem(items[slotIndex].item, items[slotIndex].quantity);

                items[slotIndex] = new InventoryItem();
                SetSelectedItem(selectedItem);
                InventoryUpdated?.Invoke();
            }
        }
        else // Если предмет уже выбран, перемещаем его
        {
            SwapOrMergeItems(slotIndex);
            ClearSelectedItem();
        }
    }

    // Метод для перемещения или объединения предметов
    private void SwapOrMergeItems(int targetSlotIndex)
    {
        var selectedItem = SelectedItem;

        var targetSlot = items[targetSlotIndex];

        if (targetSlot.item == null) // Если целевой слот пустой
        {
            targetSlot = new InventoryItem(selectedItem.item, selectedItem.quantity);
            items[targetSlotIndex] = targetSlot;
            ClearSelectedItem();
        }
        else if (targetSlot.item == selectedItem.item) // Если предметы одинаковые
        {
            int spaceLeft = targetSlot.item.maxStack - targetSlot.quantity;
            if (spaceLeft > 0)
            {
                int transferAmount = Mathf.Min(targetSlot.quantity, spaceLeft);
                targetSlot.quantity += transferAmount;
                selectedItem.quantity -= transferAmount;

                if (selectedItem.quantity <= 0) // Если предмет закончился
                {
                    ClearSelectedItem();
                }
            }
        }
        else // Если предметы разные, меняем их местами
        {
            Item tempItem = targetSlot.item;
            int tempCount = targetSlot.quantity;

            targetSlot.item = selectedItem.item;
            targetSlot.quantity = selectedItem.quantity;
            items[targetSlotIndex] = targetSlot;

            selectedItem.item = tempItem;
            selectedItem.quantity = tempCount;
            SetSelectedItem(selectedItem);
        }
        InventoryUpdated?.Invoke();
    }

    public void SetSelectedItem(InventoryItem item)
    {
        if (item == null)
            SelectedItem = new InventoryItem();
        else
        {
            SelectedItem = new InventoryItem(item.item, item.quantity);
            inventoryWindow.SetSelectedSlot(SelectedItem);
        }
    }

    public void ClearSelectedItem()
    {
        if (SelectedItem != null)
        {
            if (SelectedItem.item != null)
            {
                inventoryWindow.ClearSelectedSlot();
                SelectedItem = new InventoryItem();
            }
        }
        else
            SelectedItem = new InventoryItem();
    }

}

[System.Serializable]
public class InventoryData
{
    public List<InventoryItem> items;

    public InventoryData(List<InventoryItem> inventoryItems)
    {
        items = inventoryItems;
    }
}
