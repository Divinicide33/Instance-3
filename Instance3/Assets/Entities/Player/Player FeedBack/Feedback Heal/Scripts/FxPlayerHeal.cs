using System;
using UnityEngine;

public class FxPlayerHeal : FxElement<FxPlayerHeal>
{
    private new ParticleSystem particleSystem;

    public static Action<float> onHeal { get; set; }
    private void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }
    private void Start()
    {
        particleSystem?.Stop();
    }

    private void OnEnable()
    {
        onHeal += PlayHeal;
    }

    private void OnDisable()
    {
        onHeal -= PlayHeal;
    }

    public void PlayHeal(float healTime)
    {
        ParticleSystem.MainModule main = particleSystem.main;
        main.duration = healTime;
        Show();
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
