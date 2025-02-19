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
    public int space = 10; // ���������� ������
    public List<InventoryItem> items = new List<InventoryItem>();
    public InventoryWindow inventoryUI;
    public Transform dropPosition; // ����� ������� ��������

    void Awake()
    {
        instance = this;
        LoadInventory();
    }

    public void Add(Item item, int amount = 1)
    {
        if (!item.showInInventory) return;

        int remainingAmount = amount;

        // ��������� ��� ������������ �������� �����
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

        // ���� ��� �������� �������� � ���� �����, ������ ����� �����
        while (remainingAmount > 0 && items.Count < space)
        {
            int stackSize = Mathf.Min(remainingAmount, item.maxStack);
            items.Add(new InventoryItem(item, stackSize));
            remainingAmount -= stackSize;
        }

        if (remainingAmount > 0)
        {
            Debug.Log("��������� ����������!");
        }

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
        PlayerPrefs.SetString("Inventory", JsonUtility.ToJson(this));
        PlayerPrefs.Save();
    }

    public void LoadInventory()
    {
        if (PlayerPrefs.HasKey("Inventory"))
        {
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString("Inventory"), this);
        }
    }
}
