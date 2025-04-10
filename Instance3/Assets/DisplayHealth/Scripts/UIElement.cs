using System;
using UnityEngine;

public abstract class UIElement : MonoBehaviour
{
    public static Action onShow { get; set; }
    public static Action onHide { get; set; }
    public static Action onUpdate { get; set; }

    protected abstract void Show();
    protected abstract void Hide();
    protected abstract void UpdateDisplay();

    protected virtual void OnEnable()
    {
        onShow += Show;
        onHide += Hide;
        onUpdate += UpdateDisplay;
    }
    protected virtual void OnDisable()
    {
        onShow -= Show;
        onHide -= Hide;
        onUpdate -= UpdateDisplay;
    }

}
