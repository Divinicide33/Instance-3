using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseImage;
    [SerializeField] private float slowdownDuration = 1f; 
    [SerializeField] private GameObject pauseMenuFirstButton;


    private bool isPaused = false;
    private Coroutine pauseCoroutine;



    public void TogglePause()
    {
        if (pauseCoroutine != null) return;

        if (!isPaused)
            pauseCoroutine = StartCoroutine(SlowDownTime());
        else
            pauseCoroutine = StartCoroutine(SpeedUpTime());
    }

    private IEnumerator SlowDownTime()
    {
        isPaused = true;

        float t = 0f;
        float start = Time.timeScale;

        while (t < slowdownDuration)
        {
            t += Time.unscaledDeltaTime;
            float normalized = Mathf.Clamp01(t / slowdownDuration);
            Time.timeScale = Mathf.Lerp(start, 0f, normalized);
            yield return null;
        }

        Time.timeScale = 0f;
        pauseImage.SetActive(true);
        SetSelected(pauseMenuFirstButton);
        pauseCoroutine = null;
    }

    private IEnumerator SpeedUpTime()
    {
        pauseImage.SetActive(false);
        float t = 0f;
        float start = Time.timeScale;

        while (t < slowdownDuration)
        {
            t += Time.unscaledDeltaTime;
            float normalized = Mathf.Clamp01(t / slowdownDuration);
            Time.timeScale = Mathf.Lerp(start, 1f, normalized);
            yield return null;
        }

        Time.timeScale = 1f;
        isPaused = false;
        pauseCoroutine = null;
    }

    private void SetSelected(GameObject button)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(button);
    }
}
