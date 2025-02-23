using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HotBarItem
{
    public Item item;
    public int quantity;

    public HotBarItem(Item newItem, int amount)
    {
        item = newItem;
        quantity = amount;
    }
}
public class HotbarManager : MonoBehaviour
{
    public int hotbarSize { private set; get; } = 5;
    [SerializeField] private Transform handPosition; // Точка в руке для отображения предмета

    public int SelectedSlotIndex { private set; get; } = 0;
    private GameObject currentItemInHand; // Объект в руке
    [SerializeField]
    private List<HotBarItem> hotbarItems = new List<HotBarItem>(); // Храним предметы

    public delegate void OnHotbarUpdated();
    public event OnHotbarUpdated HotbarUpdated;

    public static HotbarManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        for (int i = 0; i < hotbarSize; i++)
        {
            hotbarItems.Add(null);
        }
    }

    private void Start()
    {
        UpdateHandItem();
    }

    private void Update()
    {
        HandleScrollInput();
    }

    private void HandleScrollInput()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll < 0f)
        {
            SelectedSlotIndex = (SelectedSlotIndex + 1) % hotbarSize;
            UpdateSelection();
        }
        else if (scroll > 0f)
        {
            SelectedSlotIndex = (SelectedSlotIndex - 1 + hotbarSize) % hotbarSize;
            UpdateSelection();
        }
    }

    public bool AddToHotbar(Item item, int count)
    {
        //for (int i = 0; i < hotbarSize; i++)
        //{
        //    if (hotbarItems[i] == null || hotbarItems[i].item == null)
        //    {
        //        hotbarItems[i] = new(item, count);
        //        HotbarUpdated?.Invoke(); // Уведомляем UI
        //        return;
        //    }
        //}

        if (hotbarItems[SelectedSlotIndex] == null || hotbarItems[SelectedSlotIndex].item == null)
        {
            hotbarItems[SelectedSlotIndex] = new(item, count);
            HotbarUpdated?.Invoke(); // Уведомляем UI
            return true;
        }
        else
            return false;

    }

    public void RemoveFromHotbar(int slotIndex)
    {
        if (hotbarItems[slotIndex] != null)
            if (hotbarItems[slotIndex].item != null)
            {
                hotbarItems[slotIndex] = null;
                HotbarUpdated?.Invoke();
            }
    }

    public List<HotBarItem> GetItems()
    {
        return hotbarItems;
    }

    private void UpdateSelection()
    {
        HotbarUpdated?.Invoke(); // Сообщаем UI об изменении
        UpdateHandItem();
    }

    private void UpdateHandItem()
    {
        HotBarItem selectedItem = hotbarItems[SelectedSlotIndex];
        var meshFilter = handPosition.GetComponent<MeshFilter>();
        var meshRender = handPosition.GetComponent<MeshRenderer>();
        if (selectedItem != null)
            if (selectedItem.item != null && selectedItem.item.prefab != null)
            {
                meshFilter.mesh = selectedItem.item.prefab.GetComponent<MeshFilter>().sharedMesh;
                meshRender.materials = selectedItem.item.prefab.gameObject.GetComponent<MeshRenderer>().sharedMaterials;
                //currentItemInHand = Instantiate(selectedItem.item.prefab, handPosition);
                //RemoveUnnecessaryComponents(currentItemInHand);
            }
            else
            {
                meshFilter.mesh = null;
                meshRender.materials = new Material[0];

            }
    }

}
