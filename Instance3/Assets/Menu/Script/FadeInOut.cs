using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class FadeInOut : MonoBehaviour
{
    public static FadeInOut Instance { get; private set; }

    [SerializeField] private CanvasGroup canvasgroup;
    [SerializeField] private float fadeDuration = 1f;

    private Coroutine currentFade;

    public static Action onUseFadeIn { get; set; }
    public static Action onUseFadeOut { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Coroutine FadeIn(Action onComplete = null)
    {
        if (currentFade != null) StopCoroutine(currentFade);
        currentFade = StartCoroutine(FadeRoutine(1, onComplete));
        return currentFade;
    }

    public Coroutine FadeOut(Action onComplete = null)
    {
        //Debug.Log("💡 FadeOut appelé !");
        canvasgroup.alpha = 1f; // force à démarrer du noir si jamais il est à 0
        if (currentFade != null) StopCoroutine(currentFade);

        currentFade = StartCoroutine(FadeRoutine(0, () =>
        {
            PlayerInputScript.onEnableInput?.Invoke();
            onComplete?.Invoke();
        }));
        return currentFade;
    }

    private IEnumerator FadeRoutine(float targetAlpha, Action onComplete)
    {
        PlayerInputScript.onDisableInput?.Invoke();

        float startAlpha = canvasgroup.alpha;
        float duration = fadeDuration; // temps total de transition
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            canvasgroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            yield return null;
        }

        canvasgroup.alpha = targetAlpha; // garantit le bon alpha final
        currentFade = null;

        if (Mathf.Approximately(targetAlpha, 0))
        {
            PlayerInputScript.onEnableInput?.Invoke();
        }

        onComplete?.Invoke();
    }

    private void OnEnable()
    {
        onUseFadeIn += () => FadeIn();
        onUseFadeOut += () => FadeOut();
    }

    private void OnDisable()
    {
        onUseFadeIn -= () => FadeIn();
        onUseFadeOut -= () => FadeOut();
    }
}
