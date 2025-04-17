/*using System.Collections;
using UnityEngine;

public class PlayerHurtFX : FxElement<PlayerHurtFX>
{

    [SerializeField] private SpriteRenderer spriteRenderer;
    private new ParticleSystem particleSystem;

    public static System.Action onHit { get; set; }

    private void Start()
    {
        //spriteRenderer.enabled = true;
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

    protected override void Enable()
    {
        spriteRenderer.enabled = true;
    }

    protected override void Disable()
    {
        spriteRenderer.enabled = false;
    }

    private void HitVFX()
    {
        particleSystem.Play();
        //StartCoroutine(HitAnim());
    }

    IEnumerator HitAnim()
    {
        int maxCount = 2;
        for (int i = 0; i < maxCount; i++)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(0.05f);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
*/