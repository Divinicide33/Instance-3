using System.Collections;
using UnityEditor.Build;
using UnityEngine;

public class DashFX : FX<DashFX>
{
    public new ParticleSystem particleSystem;

    private PlayerController player;

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
        //PlayerController.onDash += DashVFX;
    }

    protected override void OnDisable()
    {
        //PlayerController.onDash -= DashVFX;
    }

    protected override void EnableFX()
    {
    }

    protected override void DisableFX()
    {

    }

    private void DashVFX(bool dash) // add in OnEnable and OnDisable when the action in dash exists
    {
        if (dash)
        {
            int direction = player.isFacingRight ? 1 : -1;
            var shape = particleSystem.shape;
            shape.rotation = new Vector3(0, 0, direction * 90);
            particleSystem.Play();
        }
    }
}
