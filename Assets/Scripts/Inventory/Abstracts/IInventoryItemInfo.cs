
using UnityEngine;

public interface IInventoryItemInfo
{
    InventoryItemsTypes id { get; }
    string title { get; }
    string description { get; }
    int amount { get; set; }
    int maxItemsInInventorySlot { get; }
    GameObject gameObgect { get; }

    IInventoryItemInfo Clone();
}

