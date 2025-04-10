using UnityEngine;

public class FadeController : MonoBehaviour
{
    private FadeInOut fade;

    private void Start()
    {
        fade = FindObjectOfType<FadeInOut>();

        fade.FadeOut();
    }
}
