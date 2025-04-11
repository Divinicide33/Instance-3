using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    private static List<IUI> uiElements = new List<IUI>();

    public static void RegisterUI(IUI ui)
    {
        if (!uiElements.Contains(ui))
            uiElements.Add(ui);
    }

    public static void UnregisterUI(IUI ui)
    {
        if (uiElements.Contains(ui))
            uiElements.Remove(ui);
    }
    
    public static void HideAllUI()
    {
        foreach (IUI ui in uiElements)
        {
            ui.HideUI();
        }
    }

    public static void ShowAllUI()
    {
        foreach (IUI ui in uiElements)
        {
            ui.ShowUI();
        }
    }
}