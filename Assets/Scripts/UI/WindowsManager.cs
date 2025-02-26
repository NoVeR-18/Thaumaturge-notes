using Assets.Scripts.UI;
using UnityEngine;

public class WindowsManager : MonoBehaviour
{
    public static WindowsManager instance;
    public static bool windowOpened;
    public InventoryWindow _inventoryPopup;
    public QuestWindow _questsWindow;
    public ThaumaturgiconWindow _thaumaturgiconWindow;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(this);

    }

    private void Start()
    {
        windowOpened = false;
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (_questsWindow.gameObject.activeSelf)
                _questsWindow.CloseTab();
            else
                _questsWindow.OpenTab();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (_inventoryPopup.gameObject.activeSelf)
                _inventoryPopup.CloseTab();
            else
                _inventoryPopup.OpenTab();
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            if (_thaumaturgiconWindow.gameObject.activeSelf)
                _thaumaturgiconWindow.CloseTab();
            else
                _thaumaturgiconWindow.OpenTab();
        }
    }
}
