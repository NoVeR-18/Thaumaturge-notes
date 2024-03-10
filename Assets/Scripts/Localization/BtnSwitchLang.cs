using UnityEngine;

public class BtnSwitchLang : MonoBehaviour
{
    [SerializeField]
    private LocalizationManager localizationManager;

    public void OnButtonClick()
    {
        if (PlayerPrefs.GetString("Language").Equals("jp_JP"))
        {
            localizationManager.CurrentLanguage = "en_US";
            localizationManager.LoadLocalizedText("en_US");
        }
        else if (PlayerPrefs.GetString("Language").Equals("en_US"))
        {
            localizationManager.CurrentLanguage = "jp_JP";
            localizationManager.LoadLocalizedText("jp_JP");
        }

        //localizationManager.CurrentLanguage = name;
    }
}