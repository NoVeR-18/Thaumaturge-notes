using Assets.Scripts.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThaumaturgiconWindow : WindowBase
{
    [SerializeField]
    private List<Button> _headerButtons = new List<Button>();
    [SerializeField]
    private ThaumaturgiconTab _tab;

    private void Start()
    {

    }
    private void ShowTab()
    {
        _tab.RenderTab();
    }
}
