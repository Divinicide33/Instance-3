/*using System.Collections;
using UnityEngine;

public class EnemyHurtFX : FxElement<EnemyHurtFX>
{
    private new ParticleSystem particleSystem;

    [SerializeField] private SpriteRenderer spriteRenderer;
    public static System.Action<GameObject> onHit { get; set; }

    private void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        spriteRenderer = GetComponentInParent<SpriteRenderer>();
    }

    protected override void OnEnable()
    {
        onHit += PlayParticuleFX;
    }

    protected override void OnDisable()
    {
        onHit -= PlayParticuleFX;
    }

    protected override void Enable()
    {
        spriteRenderer.enabled = true;
    }

    protected override void Disable()
    {
        spriteRenderer.enabled = false;
    }

    private void PlayParticuleFX(GameObject enemy)
    {
        if (enemy.transform == gameObject.transform.parent)
        {
            particleSystem?.Play();
            StartCoroutine(HitAnim());
        }
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
}
*/