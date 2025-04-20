using UnityEngine;

public class DashFX : FxElement<DashFX>
{
    private new ParticleSystem particleSystem;

    PlayerController player;

    private void Awake()
    {
        TryGetComponent(out particleSystem);
        player = GetComponentInParent<PlayerController>();
    }

    private void Start()
    {
        particleSystem?.Stop();
    }

    private void DashVFX()
    {
        int direction = player.isFacingRight ? 1 : -1;
        var shape = particleSystem.shape;
        shape.rotation = new Vector3(0, 0, direction * 90);
        
        particleSystem?.Play();
    }

    protected override void Show()
    {
        DashVFX();
    }

    protected override void Hide()
    {
        particleSystem?.Stop();
    }

    protected override void UpdateFX()
    {
        
    }
}