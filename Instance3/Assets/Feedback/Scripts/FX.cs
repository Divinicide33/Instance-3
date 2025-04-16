using System;
using UnityEngine;

public abstract class FX<T> : MonoBehaviour, IFX where T : FX<T>
{
    public static Action onEnableFX { get; set; }
    public static Action onDisableFX { get; set; }
    protected abstract void EnableFX();
    protected abstract void DisableFX();
    protected virtual void OnEnable()
    {
        onEnableFX += EnableFX;
        onDisableFX += DisableFX;
    }

    protected virtual void OnDisable()
    {
        onEnableFX -= EnableFX;
        onDisableFX -= DisableFX;
    }

    public void Enable() => EnableFX();

    public void Disable() => DisableFX();
}
