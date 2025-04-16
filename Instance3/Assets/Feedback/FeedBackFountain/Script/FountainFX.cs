using UnityEngine;

public class FountainFX : FX<FountainFX>
{
    protected override void EnableFX()
    {
        
    }

    protected override void DisableFX()
    {

    }

    private void Start()
    {
        FountainSFX("Fountain");
    }

    private void FountainSFX(string songName)
    {
        AudioManager.OnPlaySFX?.Invoke(songName);
    }
}
