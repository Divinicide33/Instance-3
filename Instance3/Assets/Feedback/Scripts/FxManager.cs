using System.Collections.Generic;
using UnityEngine;

public class FxManager : MonoBehaviour
{
    private static List<IFX> fxElements = new List<IFX>();

    public static void RegisterFX(IFX fx)
    {
        if (!fxElements.Contains(fx))
            fxElements.Add(fx);
    }

    public static void UnregisterFX(IFX fx)
    {
        if (fxElements.Contains(fx))
            fxElements.Remove(fx);
    }

    public static void HideAllFX()
    {
        foreach (IFX fx in fxElements)
        {
            fx.HideUI();
        }
    }

    public static void ShowAllFX()
    {
        foreach (IFX fx in fxElements)
        {
            fx.ShowUI();
        }
    }
}