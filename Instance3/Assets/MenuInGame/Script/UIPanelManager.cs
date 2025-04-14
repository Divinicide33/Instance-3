using UnityEngine;

public class UIPanelManager
{
    private GameObject[] allPanels;

    public UIPanelManager(params GameObject[] panels)
    {
        allPanels = panels;
    }

    public void ShowOnly(GameObject panelToShow)
    {
        foreach (var panel in allPanels)
            panel.SetActive(panel == panelToShow);
    }

    public void HideAll()
    {
        foreach (var panel in allPanels)
            panel.SetActive(false);
    }
}
