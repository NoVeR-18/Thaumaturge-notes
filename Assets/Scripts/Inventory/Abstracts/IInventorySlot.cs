public interface IInventorySlot
{
    bool isFull { get; }
    bool isEmpty { get; }

    public IInventoryItemInfo item { get; }
    InventoryItemsTypes itemType { get; }
    int amount { get; }
    int capacity { get; }

    void SetItem(IInventoryItemInfo item);
    void Clear();

}