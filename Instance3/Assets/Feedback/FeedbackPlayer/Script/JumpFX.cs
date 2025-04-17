/*using System;
using UnityEngine;

public class JumpFX : FxElement<JumpFX>
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

    protected override void Enable()
    {
    }

    protected override void Disable()
    {

    }

    private void JumpVFX()
    {
          particleSystem.Play();
    }
}
*/