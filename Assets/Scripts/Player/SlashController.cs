using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SlashController : MonoBehaviour
{
    [SerializeField] private VisualEffect slashEffect;

    private void Start()
    {
        slashEffect.enabled = false;
    }

    public void TriggerSlash()
    {
        slashEffect.enabled = true;
        slashEffect.Play();
    }

    public void StopSlash()
    {
        slashEffect.enabled = false;
    }
}
