using System;
using UnityEngine;

public abstract class UIElement<T> : MonoBehaviour, IUI where T : UIElement<T>
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
        UiManager.RegisterUI(this);
    }
    
    protected virtual void OnDisable()
    {
        onShow -= Show;
        onHide -= Hide;
        onUpdate -= UpdateDisplay;
    }

    public void HideUI() => Hide();
    public void ShowUI() => Show();
}
