using UnityEngine;

public abstract class FxElement<T> : MonoBehaviour where T : FxElement<T>
{
    protected abstract void Show();
    protected abstract void Hide();
    protected abstract void UpdateFX();

    private void OnEnable()
    {
        Init();
    }

    private void OnDisable()
    {
        Destroy();
    }

    protected virtual void Init() { }
    protected virtual void Destroy() { }

    public void ShowFX() => Show();
    public void HideFX() => Hide();
    public void RefreshFX() => UpdateFX();
}
