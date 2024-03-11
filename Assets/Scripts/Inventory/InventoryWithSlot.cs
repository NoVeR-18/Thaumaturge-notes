using System;
using System.Collections.Generic;
using System.Linq;
using Debug = UnityEngine.Debug;

public class InventoryWithSlot : IInventory
{
    public event Action<object, IInventoryItem, int> OnInventoryItemAddedEvent;
    public event Action<object, Type, int> OnInventoryItemRemovedEvent;
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

    public IInventoryItem GetItem(Type itemType)
    {
        return _slots.Find(slot => slot.itemType == itemType).item;
    }

    public IInventoryItem[] GetAllItems()
    {
        var allItems = new List<IInventoryItem>();
        foreach (var slot in _slots)
        {
            if (!slot.isEmpty)
                allItems.Add(slot.item);
        }
        return allItems.ToArray();
    }

    public IInventoryItem[] GetAllItems(Type itemType)
    {
        var allItemsOfType = new List<IInventoryItem>();
        var slotsOfType = _slots.FindAll(slot => !slot.isEmpty && slot.itemType == itemType);
        foreach (var slot in slotsOfType)
            allItemsOfType.Add(slot.item);
        return allItemsOfType.ToArray();
    }

    public IInventoryItem[] GetEquippedItem()
    {
        var requiredSlots = _slots.FindAll(slot => slot.item.state.isEquipped);
        var equippedItems = new List<IInventoryItem>();
        foreach (var slot in requiredSlots)
            equippedItems.Add(slot.item);
        return equippedItems.ToArray();
    }


    public int GetItemAmount(Type itemType)
    {
        var amount = 0;
        var allItemsSlots = _slots.FindAll(slot => !slot.isEmpty && slot.itemType == itemType);
        foreach (var slot in allItemsSlots)
            amount += slot.amount;
        return amount;
    }

    public bool HasItem(Type type, out IInventoryItem item)
    {
        item = GetItem(type);
        return item != null;
    }

    public void Remove(object sender, Type itemType, int amount = 1)
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
                slot.item.state.amount -= amountToRemove;

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

    public IInventorySlot[] GetAllSlots(Type itemType)
    {
        return _slots.FindAll(slot => !slot.isEmpty && slot.itemType == itemType).ToArray();
    }
    public IInventorySlot[] GetAllSlots()
    {
        return _slots.ToArray();
    }

    public bool TryToAdd(object sender, IInventoryItem item)
    {
        var slotWithSameItemButNotEmpty = _slots.Find(slot => !slot.isEmpty
                                                        && slot.itemType == item.type && !slot.isFull);
        if (slotWithSameItemButNotEmpty != null)
            return TryToAddToSlot(sender, slotWithSameItemButNotEmpty, item);
        var emptySlot = _slots.Find(slot => slot.isEmpty);
        if (emptySlot != null)
            return TryToAddToSlot(sender, emptySlot, item);
        Debug.Log($"Cannot add item ({item.type}), amount: {item.state.amount}, " +
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
        toSlot.item.state.amount += amountToAdd;
        if (fits)
            fromSlot.Clear();
        else
            fromSlot.item.state.amount = amountLeft;
        OnInventoryStateChangedEvent?.Invoke(sender);
    }


    private bool TryToAddToSlot(object sender, IInventorySlot slot, IInventoryItem item)
    {
        var fits = slot.amount + item.state.amount <= item.info.maxItemsInInventorySlot;
        var amountToAdd = fits ? item.state.amount : item.info.maxItemsInInventorySlot - slot.amount;
        var amountLeft = item.state.amount - amountToAdd;
        var clonedItem = item.Clone();
        clonedItem.state.amount = amountLeft;

        if (slot.isEmpty)
            slot.SetItem(clonedItem);
        else
            slot.item.state.amount += amountToAdd;

        OnInventoryItemAddedEvent?.Invoke(sender, item, amountToAdd);
        OnInventoryStateChangedEvent?.Invoke(sender);
        if (amountLeft <= 0)
            return true;
        item.state.amount = amountLeft;
        return TryToAdd(sender, item);
    }
}
