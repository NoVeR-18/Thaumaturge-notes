using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public Item item;
    public int quantity;

    public InventoryItem(Item newItem, int amount)
    {
        item = newItem;
        quantity = amount;
    }
}

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public int space = 10; // Количество слотов
    public List<InventoryItem> items = new List<InventoryItem>();
    public Transform dropPosition; // Точка выброса предмета

    public Action InventoryUpdated;

    void Awake()
    {
        instance = this;
        LoadInventory();
    }

    public void Add(Item item, int amount = 1)
    {
        if (!item.showInInventory) return;

        int remainingAmount = amount;

        // Заполняем уже существующие неполные стеки
        foreach (var inventoryItem in items)
        {
            if (inventoryItem.item == item && inventoryItem.quantity < item.maxStack)
            {
                int addable = Mathf.Min(item.maxStack - inventoryItem.quantity, remainingAmount);
                inventoryItem.quantity += addable;
                remainingAmount -= addable;

                if (remainingAmount <= 0)
                    break;
            }
        }

        // Если ещё остались предметы и есть место, создаём новые стеки
        while (remainingAmount > 0 && items.Count < space)
        {
            int stackSize = Mathf.Min(remainingAmount, item.maxStack);
            items.Add(new InventoryItem(item, stackSize));
            remainingAmount -= stackSize;
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
        InventoryItem existingItem = items.Find(i => i.item == item);
        if (existingItem != null)
        {
            existingItem.quantity -= amount;
            if (existingItem.quantity <= 0)
                items.Remove(existingItem);
        }
        SaveInventory();
    }

    public void DropItem(Item item)
    {
        InventoryItem existingItem = items.Find(i => i.item == item);
        if (existingItem != null)
        {
            GameObject droppedItem = Instantiate(item.prefab, dropPosition.position, Quaternion.identity);
            Remove(item);
        }
    }
    public int GetItemCount(InventoryItem item)
    {
        return item.quantity;
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
        }
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
