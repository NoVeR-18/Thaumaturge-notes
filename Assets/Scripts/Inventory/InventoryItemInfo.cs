using UnityEngine;

[CreateAssetMenu(fileName = "InventoryItemInfo", menuName = "Gameplay/Items/Create new ItemInfo")]
public class InventoryItemInfo : ScriptableObject, IInventoryItemInfo
{
    [SerializeField] private InventoryItemsTypes _id;
    [SerializeField] public string _title;
    [SerializeField] public string _description;
    [SerializeField] public int _maxItemsInInventorySlot;
    [SerializeField] public GameObject _mesh;


    public InventoryItemsTypes id => _id;

    public string title => _title = _id.ToString();

    public string description => _description;

    public int maxItemsInInventorySlot => _maxItemsInInventorySlot;

    public GameObject gameObgect => _mesh;

    public int amount { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public IInventoryItemInfo Clone()
    {
        var clonedApple = this;
        clonedApple.amount = amount;
        return clonedApple;
    }
}