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
    public static Inventory instance;
    public int space = 10; // ���������� ������
    public List<InventoryItem> items = new List<InventoryItem>();
    public Transform dropPosition; // ����� ������� ��������

    public Action InventoryUpdated;

    void Awake()
    {
        instance = this;
        InitializeInventory();
        LoadInventory();
    }

    private void InitializeInventory()
    {
        items = new List<InventoryItem>();
        for (int i = 0; i < space; i++)
        {
            items.Add(new InventoryItem()); // ��������� ������� �������
        }
    }

    public void Add(Item item, int amount = 1)
    {
        int remainingAmount = amount;

        // ��������� ������������ �����
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

        // ���������� � ������ �����
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
            Debug.Log("��������� ����������!");
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

            // ���� ����������� ��������� ������ ������� �������, ��������� ������� �������
            while (items.Count < space)
            {
                items.Add(new InventoryItem());
            }
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
