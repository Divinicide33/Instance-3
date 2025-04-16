using System;
using UnityEngine;

public class DashFX : FX<DashFX>
{
    public new ParticleSystem particleSystem;

    PlayerController player;

    public static Action onDash { get; set; }

    private void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();
        player = GetComponentInParent<PlayerController>();
    }

    private void Start()
    {
        particleSystem.Stop();
    }
    protected override void OnEnable()
    {
        onDash += DashVFX;
    }

    protected override void OnDisable()
    {
        onDash -= DashVFX;
    }

    protected override void EnableFX()
    {
    }

    protected override void DisableFX()
    {

    }

    private void DashVFX()
    {
        int direction = player.isFacingRight ? 1 : -1;
        var shape = particleSystem.shape;
        shape.rotation = new Vector3(0, 0, direction * 90);
        particleSystem.Play();
    }
}
