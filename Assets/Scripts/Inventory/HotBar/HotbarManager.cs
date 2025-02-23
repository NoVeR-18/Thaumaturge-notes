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
    [SerializeField] private Transform handPosition;

    public int SelectedSlotIndex { private set; get; } = 0;
    private GameObject currentItemInHand;
    [SerializeField]
    private List<HotBarItem> hotbarItems = new List<HotBarItem>();

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
        LoadHotbar();
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
            HotbarUpdated?.Invoke();

            UpdateHandItem();
            SaveHotbar();
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
        HotbarUpdated?.Invoke();
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
            }
            else
            {
                meshFilter.mesh = null;
                meshRender.materials = new Material[0];

            }
    }
    public void SaveHotbar()
    {
        string json = JsonUtility.ToJson(new HotbarData(hotbarItems));
        PlayerPrefs.SetString("Hotbar", json);
        PlayerPrefs.Save();
    }

    public void LoadHotbar()
    {
        if (PlayerPrefs.HasKey("Hotbar"))
        {
            string json = PlayerPrefs.GetString("Hotbar");
            HotbarData data = JsonUtility.FromJson<HotbarData>(json);

            hotbarItems = data.items;

            HotbarUpdated?.Invoke();
        }
    }
}
[System.Serializable]
public class HotbarData
{
    public List<HotBarItem> items = new();

    public HotbarData(List<HotBarItem> hotbarItems)
    {
        foreach (var hotbarItem in hotbarItems)
        {
            if (hotbarItem != null && hotbarItem.item != null)
                items.Add(new HotBarItem(hotbarItem.item, hotbarItem.quantity));
            else
                items.Add(null);
        }
    }
}
