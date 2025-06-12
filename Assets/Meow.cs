using UnityEngine;

public class Meow : MonoBehaviour
{
    public AudioClip[] MeowSoundEffecs;
    private AudioClip ChosenMeow;
    private AudioSource Audio;

    private void Awake()
    {
        Audio = GetComponent<AudioSource>();
    }

    public void PlayMeow()
    {
        ChosenMeow = MeowSoundEffecs[Random.Range(0, MeowSoundEffecs.Length)];
        Audio.clip = ChosenMeow;
        Audio.Play();
    }
}
