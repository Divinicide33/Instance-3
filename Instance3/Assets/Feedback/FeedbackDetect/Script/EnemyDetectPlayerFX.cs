using UnityEngine;

public class EnemyDetectPlayerFX : FxElement<EnemyDetectPlayerFX>
{
    private GameObject detect;

    private void Awake()
    {
        detect = transform.GetChild(0).gameObject;
        detect.SetActive(false);
    }

    protected override void Show()
    {
        detect.SetActive(true);
    }

    protected override void Hide()
    {
        detect.SetActive(false);
    }

    protected override void UpdateFX()
    {
        // Exemple : changer la couleur, animation, etc.
    }
}
