using System;
using UnityEngine;

public class JumpFX : FX<JumpFX>
{
    private new ParticleSystem particleSystem;

    public static Action onJump { get; set; }

    bool resetJump;
    private void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    private void Start()
    {
        particleSystem.Stop();
    }
    protected override void OnEnable()
    {
        onJump += JumpVFX;
    }

    protected override void OnDisable()
    {
        onJump -= JumpVFX;
    }

    protected override void EnableFX()
    {
    }

    protected override void DisableFX()
    {

    }

    private void JumpVFX()
    {
          particleSystem.Play();
    }
}
