using System;
using System.Collections.Generic;
using System.Linq;
using Debug = UnityEngine.Debug;

public class InventoryWithSlot
{
    public event Action<object, IInventoryItemInfo, int> OnInventoryItemAddedEvent;
    public event Action<object, IInventoryItemInfo, int> OnInventoryItemRemovedEvent;
    public event Action<object> OnInventoryStateChangedEvent;

    public int Capacity { get; set; }

    public bool IsFull => _slots.All(slot => slot.isFull);

    private List<IInventorySlot> _slots;

    public InventoryWithSlot(int capacity)
    {
        this.Capacity = capacity;

        _slots = new List<IInventorySlot>(capacity);
        for (int i = 0; i < capacity; i++)
            _slots.Add(new InventorySlot());
    }

    public IInventoryItemInfo GetItem(IInventoryItemInfo itemType)
    {
        return _slots.Find(slot => slot.itemType == itemType.id).item;
    }

    public IInventoryItemInfo[] GetAllItems()
    {
        var allItems = new List<IInventoryItemInfo>();
        foreach (var slot in _slots)
        {
            if (!slot.isEmpty)
                allItems.Add(slot.item);
        }
        return allItems.ToArray();
    }

    public IInventoryItemInfo[] GetAllItems(IInventoryItemInfo itemType)
    {
        var allItemsOfType = new List<IInventoryItemInfo>();
        var slotsOfType = _slots.FindAll(slot => !slot.isEmpty && slot.itemType == itemType.id);
        foreach (var slot in slotsOfType)
            allItemsOfType.Add(slot.item);
        return allItemsOfType.ToArray();
    }

    public int GetItemAmount(IInventoryItemInfo itemType)
    {
        var amount = 0;
        var allItemsSlots = _slots.FindAll(slot => !slot.isEmpty && slot.itemType == itemType.id);
        foreach (var slot in allItemsSlots)
            amount += slot.amount;
        return amount;
    }

    public bool HasItem(IInventoryItemInfo type, out IInventoryItemInfo item)
    {
        item = GetItem(type);
        return item != null;
    }

    public void Remove(object sender, IInventoryItemInfo itemType, int amount = 1)
    {
        var slotWithItem = GetAllSlots(itemType);
        if (slotWithItem.Length == 0)
            return;

        var amountToRemove = amount;
        var count = slotWithItem.Length;

        for (int i = count - 1; i >= 0; i--)
        {
            var slot = slotWithItem[i];
            if (slot.amount <= amountToRemove)
            {
                slot.item.amount -= amountToRemove;

                if (slot.amount <= 0)
                    slot.Clear();

                OnInventoryItemRemovedEvent?.Invoke(sender, itemType, amountToRemove);
                OnInventoryStateChangedEvent?.Invoke(sender);
                break;
            }

            var amountRemoved = slot.amount;
            amountToRemove -= slot.amount;
            slot.Clear();
            OnInventoryItemRemovedEvent?.Invoke(sender, itemType, amountToRemove);
            OnInventoryStateChangedEvent?.Invoke(sender);
        }
    }

    public IInventorySlot[] GetAllSlots(IInventoryItemInfo itemType)
    {
        return _slots.FindAll(slot => !slot.isEmpty && slot.itemType == itemType.id).ToArray();
    }
    public IInventorySlot[] GetAllSlots()
    {
        return _slots.ToArray();
    }

    public bool TryToAdd(object sender, IInventoryItemInfo item)
    {
        var slotWithSameItemButNotEmpty = _slots.Find(slot => !slot.isEmpty
                                                        && slot.itemType == item.id && !slot.isFull);
        if (slotWithSameItemButNotEmpty != null)
            return TryToAddToSlot(sender, slotWithSameItemButNotEmpty, item);
        var emptySlot = _slots.Find(slot => slot.isEmpty);
        if (emptySlot != null)
            return TryToAddToSlot(sender, emptySlot, item);
        Debug.Log($"Cannot add item ({item.id}), amount: {item.amount}, " +
            $"because there is no place for that.");
        return false;
    }

    public void TransitFromSlotToSlot(object sender, IInventorySlot fromSlot, IInventorySlot toSlot)
    {
        if (fromSlot.isEmpty)
            return;
        if (toSlot.isFull)
            return;
        if (!toSlot.isEmpty && fromSlot.itemType != toSlot.itemType)
            return;


        var slotCapacity = fromSlot.capacity;
        var fits = fromSlot.amount + toSlot.amount <= slotCapacity;
        var amountToAdd = fits ? fromSlot.amount : Capacity - toSlot.amount;
        var amountLeft = fromSlot.amount - amountToAdd;
        if (toSlot.isEmpty)
        {
            toSlot.SetItem(fromSlot.item);
            fromSlot.Clear();
            OnInventoryStateChangedEvent?.Invoke(sender);
        }
        toSlot.item.amount += amountToAdd;
        if (fits)
            fromSlot.Clear();
        else
            fromSlot.item.amount = amountLeft;
        OnInventoryStateChangedEvent?.Invoke(sender);
    }


    private bool TryToAddToSlot(object sender, IInventorySlot slot, IInventoryItemInfo item)
    {
        var fits = slot.amount + item.amount <= item.maxItemsInInventorySlot;
        var amountToAdd = fits ? item.amount : item.maxItemsInInventorySlot - slot.amount;
        var amountLeft = item.amount - amountToAdd;
        var clonedItem = item.Clone();
        clonedItem.amount = amountLeft;

        if (slot.isEmpty)
            slot.SetItem(clonedItem);
        else
            slot.item.amount += amountToAdd;

        OnInventoryItemAddedEvent?.Invoke(sender, item, amountToAdd);
        OnInventoryStateChangedEvent?.Invoke(sender);
        if (amountLeft <= 0)
            return true;
        item.amount = amountLeft;
        return TryToAdd(sender, item);
    }
}
