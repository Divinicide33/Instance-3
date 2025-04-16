using UnityEngine;

public class EnemyHurtFX : FX<EnemyHurtFX>
{
    private new ParticleSystem particleSystem;
    public static System.Action onHit { get; set; }
    private void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }
    protected override void OnEnable()
    {
        onHit += HitVFX;
    }
    protected override void OnDisable()
    {
        onHit -= HitVFX;
    }
    protected override void EnableFX()
    {
    }
    protected override void DisableFX()
    {
    }
    private void HitVFX()
    {
        particleSystem.Play();
    }

}
