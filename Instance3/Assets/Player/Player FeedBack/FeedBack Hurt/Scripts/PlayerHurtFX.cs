using System.Collections;
using UnityEngine;

public class PlayerHurtFX : FxElement<PlayerHurtFX>
{
    [SerializeField] private int maxCount = 2;
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    private new ParticleSystem particleSystem;

    private void Start()
    {
        spriteRenderer.enabled = true;
        TryGetComponent(out particleSystem);
    }

    private void HitVFX()
    {
        particleSystem?.Play();
        StartCoroutine(HitAnim());
    }

    IEnumerator HitAnim()
    {
        for (int i = 0; i < maxCount; i++)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(0.05f);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(0.05f);
        }
    }

    protected override void Show()
    {
        HitVFX();
    }

    protected override void Hide()
    {
    }

    protected override void UpdateFX()
    {
    }
}