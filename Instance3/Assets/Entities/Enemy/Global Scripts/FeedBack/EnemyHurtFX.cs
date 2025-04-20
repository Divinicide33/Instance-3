using System.Collections;
using UnityEngine;

public class EnemyHurtFX : FxElement<EnemyHurtFX>
{
    private new ParticleSystem particleSystem;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        TryGetComponent(out particleSystem);
        spriteRenderer = GetComponentInParent<SpriteRenderer>();
    }

    private void PlayParticuleFX()
    {
        particleSystem?.Play();
        StartCoroutine(HitAnim());
    }

    IEnumerator HitAnim()
    {
        int maxCount = 2;
        Color color = spriteRenderer.color;
        for (int i = 0; i < maxCount; i++)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.05f);
            spriteRenderer.color = color;
            yield return new WaitForSeconds(0.05f);
        }
    }

    protected override void Show()
    {
        PlayParticuleFX();
    }

    protected override void Hide()
    {
    }

    protected override void UpdateFX()
    {
    }
}