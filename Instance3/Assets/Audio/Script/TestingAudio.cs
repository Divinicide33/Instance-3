using UnityEngine;

public class TestingAudio : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            AudioManager.OnPlaySFX?.Invoke("HarpyAttack");
        }

        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            AudioManager.OnPlaySFX?.Invoke("M1");
        }

        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            AudioManager.OnPlaySFX?.Invoke("M2");
        }

        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            AudioManager.OnPlaySFX?.Invoke("MusicTest");
        }
    }
}
