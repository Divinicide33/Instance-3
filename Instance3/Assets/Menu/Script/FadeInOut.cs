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
    public static Action onResetCurrentFade { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            if (canvasgroup == null)
            canvasgroup = GetComponentInChildren<CanvasGroup>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Coroutine FadeIn(Action onComplete = null)
    {
        //Debug.Log($"FadeIn : at the start currentFade = {currentFade}");

        if (currentFade != null) 
        {
            Debug.Log("FadeIn : currentFade != null");
            StopCoroutine(currentFade);
        }

        //Debug.Log($"FadeIn : onComplete = {onComplete}");

        currentFade = StartCoroutine(FadeRoutine(1, onComplete));

        //Debug.Log($"FadeIn : currentFade will return {currentFade}");
        return currentFade;
    }

    public Coroutine FadeOut(Action onComplete = null)
    {
        //Debug.Log("💡 FadeOut appelé !");
        //Debug.Log($"FadeOut : at the start currentFade = {currentFade}");

        canvasgroup.alpha = 1f; // force à démarrer du noir si jamais il est à 0

        if (currentFade != null) 
        {
            Debug.Log("FadeOut : currentFade != null");
            StopCoroutine(currentFade);
        }
        
        //Debug.Log($"FadeIn : onComplete = {onComplete}");

        currentFade = StartCoroutine(FadeRoutine(0, () =>
        {
            PlayerInputScript.onEnableInput?.Invoke();
            onComplete?.Invoke();
        }));
        
        //Debug.Log($"FadeOut : currentFade will return {currentFade}");

        return currentFade;
    }

    private IEnumerator FadeRoutine(float targetAlpha, Action onComplete)
    {
        Debug.Log("💡 FadeRoutine is called !");
        //Debug.Log($"FadeRoutine : onComplete = {onComplete}");

        if (canvasgroup == null)
        {
            Debug.LogError("CanvasGroup is null in FadeRoutine!");
            yield break;
        }
        

        PlayerInputScript.onDisableInput?.Invoke();

        float startAlpha = canvasgroup.alpha;
        float duration = fadeDuration; // temps total de transition
        float elapsed = 0f;

        //Debug.Log($"duration = {duration}");
        //Debug.Log($"elapsed = {elapsed}");

        while (elapsed < duration)
        {
            //Debug.Log($"duration = {duration}");
            //Debug.Log($"elapsed = {elapsed}");
    
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            canvasgroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            yield return null;
        }

        canvasgroup.alpha = targetAlpha; // garantit le bon alpha final

        ResetCurrentFade();

        if (Mathf.Approximately(targetAlpha, 0))
        {
            PlayerInputScript.onEnableInput?.Invoke();
        }

        onComplete?.Invoke();
    }

    private void ResetCurrentFade()
    {
        currentFade = null;
        Debug.Log($"currentFade = {currentFade}");
    }

    private void OnEnable()
    {
        onUseFadeIn += () => FadeIn();
        onUseFadeOut += () => FadeOut();
        onResetCurrentFade += () => ResetCurrentFade();
    }

    private void OnDisable()
    {
        onUseFadeIn -= () => FadeIn();
        onUseFadeOut -= () => FadeOut();
        onResetCurrentFade -= () => ResetCurrentFade();
    }
}
