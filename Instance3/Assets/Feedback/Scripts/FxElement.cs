using UnityEngine;

public abstract class FxElement<T> : MonoBehaviour where T : FxElement<T>
{
    protected abstract void Show();
    protected abstract void Hide();
    protected abstract void UpdateFX();

    private void OnEnable()
    {
        OnFxInit();
    }

    private void OnDisable()
    {
        OnFxDestroy();
    }

    protected virtual void OnFxInit() { }
    protected virtual void OnFxDestroy() { }

    public void ShowFX() => Show();
    public void HideFX() => Hide();
    public void RefreshFX() => UpdateFX();
}
