using System;
using UnityEngine;

public class JumpFX : FxElement<JumpFX>
{
    private new ParticleSystem particleSystem;

    bool resetJump;
    private void Awake()
    {
        TryGetComponent(out particleSystem);
    }

    private void Start()
    {
        particleSystem?.Stop();
    }

    protected override void Show()
    {
        particleSystem?.Play();
    }

    protected override void Hide()
    {
    }

    protected override void UpdateFX()
    {
    }
}
