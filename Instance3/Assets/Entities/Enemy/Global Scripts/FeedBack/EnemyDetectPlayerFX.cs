using UnityEngine;

public class EnemyDetectPlayerFX : FxElement<EnemyDetectPlayerFX>
{
    private GameObject detect;
    [SerializeField] private Animator animator;

    private void Awake()
    {
        detect = transform.GetChild(0).gameObject;
        detect.SetActive(false);
    }

    protected override void Show()
    {
        animator.SetBool("Attack?",true);
        detect.SetActive(true);
    }

    protected override void Hide()
    {
        animator.SetBool("Attack?", false);
        detect.SetActive(false);
    }

    protected override void UpdateFX()
    {
        // Exemple : changer la couleur, animation, etc.
    }
}
