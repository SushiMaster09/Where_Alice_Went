using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    [SerializeField] private Color flashColour = Color.white;
    [SerializeField] private float flashTime = 0.25f;

    private Color spriteColour;
    private SpriteRenderer[] spriteRenderers;
    private Material[] materials;

    private Coroutine damageFlashCoroutine;

    private void Awake()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        Init();
    }

    private void Init()
    {
        materials = new Material[spriteRenderers.Length];

        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            materials[i] = spriteRenderers[i].material;
            spriteColour = spriteRenderers[i].color;
        }
    }

    public void CallDamageFlash()
    {
        damageFlashCoroutine = StartCoroutine(DamageFlasher());
    }

    private IEnumerator DamageFlasher()
    {
        SetFlashColour();

        float currentFlashAmount = 0f;
        float elapsedTime = 0f;

        Color currentSpriteColour = Color.white;

        while (elapsedTime < flashTime)
        {
            elapsedTime += Time.deltaTime;

            currentFlashAmount = Mathf.Lerp(1f, 0f, (elapsedTime / flashTime));
            currentSpriteColour = Color.Lerp(Color.white, spriteColour, (elapsedTime / flashTime));

            SetFlashAmount(currentFlashAmount);
            SetSpriteColour(currentSpriteColour);

            yield return null;
        }
    }

    private void SetFlashColour()
    {
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].SetColor("_FlashColour", flashColour);
        }
    }

    private void SetFlashAmount(float amount)
    {
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].SetFloat("_FlashAmount", amount);
        }
    }

    private void SetSpriteColour(Color colour)
    {
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].color = colour;
        }
    }
}
