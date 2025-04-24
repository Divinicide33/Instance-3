using UnityEngine;

public abstract class FxElement<T> : MonoBehaviour where T : FxElement<T>
{
    protected abstract void Show();
    protected virtual void Show(string sfxName) 
    {
        //Debug.Log($"Play SFX: {sfxName}");
        AudioManager.OnPlaySFX?.Invoke(sfxName);
    }
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

    public void ShowVFX() => Show();
    public void ShowSFX(string sfxName) => Show(sfxName);
    public void HideFX() => Hide();
    public void RefreshFX() => UpdateFX();
}
