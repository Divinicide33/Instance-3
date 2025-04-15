using System.Collections;
using UnityEditor.Build;
using UnityEngine;

public class DashFX : FX<DashFX>
{
    public new ParticleSystem particleSystem;

    private PlayerController player;
    private PlayerDash playerDash;
    private bool isDashingField;

    private void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();
        player = GetComponentInParent<PlayerController>();
        playerDash = GetComponentInParent<PlayerDash>();
    }

    private void Start()
    {
        particleSystem.Stop();
        //isDashingField = PlayerDash.onSetIsDashing; 
    }
    protected override void OnEnable()
    {
        PlayerController.onDash += DashVFX;
    }

    protected override void OnDisable()
    {
        PlayerController.onDash -= DashVFX;
    }

    protected override void EnableFX()
    {
    }

    protected override void DisableFX()
    {

    }

    private void DashVFX()
    {
        //bool haveDash = false;
        //if (IsDashing())
        //{
        //    haveDash = true;
        //}

        int direction = player.isFacingRight ? 1 : -1;
        var shape = particleSystem.shape;
        shape.rotation = new Vector3(0, 0, direction * 90);
        //if (haveDash == true)
        //{
        particleSystem.Play();
        //    haveDash = false;
        //}

    }
}
