using UnityEngine;
using System.Collections;

public class SpriteSwapper : MonoBehaviour
{
    public GameObject targetObject;    // Object with the SpriteRenderer to affect
    public Sprite newSprite;           // Sprite to show when triggered
    public float fadeDuration = 0.5f;  // Duration of fade-in and fade-out

    private Sprite originalSprite;
    private SpriteRenderer targetRenderer;
    private Coroutine fadeCoroutine;

    void Start()
    {
        if (targetObject != null)
        {
            targetRenderer = targetObject.GetComponent<SpriteRenderer>();
            if (targetRenderer != null)
            {
                originalSprite = targetRenderer.sprite;
            }
            else
            {
                Debug.LogError("Target object does not have a SpriteRenderer.");
            }
        }
        else
        {
            Debug.LogError("Target object not assigned.");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (targetRenderer != null && newSprite != null)
        {
            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(FadeToSprite(newSprite));
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (targetRenderer != null)
        {
            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(FadeToSprite(originalSprite));
        }
    }

    private IEnumerator FadeToSprite(Sprite targetSprite)
    {
        float elapsed = 0f;
        Color color = targetRenderer.color;

        // Step 1: Fade out
        while (elapsed < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            targetRenderer.color = new Color(color.r, color.g, color.b, alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }

        targetRenderer.color = new Color(color.r, color.g, color.b, 0f);
        targetRenderer.sprite = targetSprite;

        // Step 2: Fade in
        elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);
            targetRenderer.color = new Color(color.r, color.g, color.b, alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }

        targetRenderer.color = new Color(color.r, color.g, color.b, 1f);
    }
}
