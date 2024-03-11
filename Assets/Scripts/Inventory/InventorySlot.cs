public class InventorySlot : IInventorySlot
{
    //private int _amount;

    public bool isFull => amount == capacity;

    public bool isEmpty => item == null;

    public IInventoryItemInfo item { get; private set; }

    public InventoryItemsTypes itemType => item.id;

    public int amount => isEmpty ? 0 : item.amount;

    public int capacity { get; private set; }

    public void SetItem(IInventoryItemInfo item)
    {
        if (!isEmpty)
            return;
        this.item = item;
        this.capacity = item.maxItemsInInventorySlot;
    }

    public void Clear()
    {
        if (isEmpty) return;
        item.amount = 0;
        item = null;
    }
}
