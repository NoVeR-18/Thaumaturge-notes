using Assets.Scripts.UI;

public class InventoryWindow : WindowBase
{
    private InventorySlot[] slots;

    private void Awake()
    {
        slots = GetComponentsInChildren<InventorySlot>();
    }

    public override void OpenTab()
    {
        base.OpenTab();
        UpdateUI();
    }

    public void UpdateUI()
    {
        var items = Inventory.instance.items;
        int itemCount = items.Count;

        for (int i = 0; i < slots.Length; i++)
        {
            if (i < itemCount)
            {
                slots[i].AddItem(items[i].item, Inventory.instance.GetItemCount(items[i]));
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }
}