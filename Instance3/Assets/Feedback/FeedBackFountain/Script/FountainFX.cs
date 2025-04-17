using UnityEngine;

public class FountainFX : FxElement<FountainFX>
{

    private void Start()
    {
        FountainSFX("Fountain");
    }

    private void FountainSFX(string songName)
    {
        AudioManager.OnPlaySFX?.Invoke(songName);
    }

    protected override void Show()
    {
    }

    protected override void Hide()
    {
    }

    protected override void UpdateFX()
    {
    }
}