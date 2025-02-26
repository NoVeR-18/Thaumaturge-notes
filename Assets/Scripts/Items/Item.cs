using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public GameObject prefab;
    public int maxStack = 1;

    public virtual void Use()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            var usedItem = HotbarManager.Instance.hotbarItems[HotbarManager.Instance.SelectedSlotIndex];
            if (usedItem != null)
            {
                if (usedItem.item != null)
                {
                    HotbarManager.Instance.RemoveItemFromHotbar(HotbarManager.Instance.hotbarItems[HotbarManager.Instance.SelectedSlotIndex]);
                }
            }
        }
    }
}