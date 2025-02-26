using UnityEngine;

namespace Assets.Scripts.UI
{
    public class WindowBase : MonoBehaviour
    {
        virtual public void CloseTab()
        {
            var parrent = GetComponent<Transform>();
            parrent.gameObject.SetActive(false);
            WindowsManager.windowOpened = false;
        }
        virtual public void OpenTab()
        {
            var parrent = GetComponent<Transform>();
            WindowsManager.windowOpened = true;
            parrent.gameObject.SetActive(true);
        }
    }
}
