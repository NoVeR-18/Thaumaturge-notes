using UnityEngine;


public class InventoryItemInfo : ScriptableObject, IInventoryItemInfo
{
    [SerializeField] private string _id;
    [SerializeField] public string _title;
    [SerializeField] public string _description;
    [SerializeField] public int _maxItemsInInventorySlot;
    [SerializeField] public Sprite _spriteIcon;


    public string id => _id;

    public string title => _title;

    public string description => _description;

    public int maxItemsInInventorySlot => _maxItemsInInventorySlot;

    public Sprite spriteIcon => _spriteIcon;
}