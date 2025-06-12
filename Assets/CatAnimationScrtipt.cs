using System.Collections;
using UnityEngine;

public class CatAnimationScrtipt : MonoBehaviour
{
    private Animator animator;
    private string[] AnimationTriggers = new string[] { "Meow","EarWiggle","Blink","LeftEarWiggle","TailSwish","TailMove"};
    private int RandomAnimationIntival;
    private int BlinkIntival;


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        StartCoroutine(PlayRandomCatAnimation());
        StartCoroutine(Blink());
    }

    public void TriggerAnimation(string TriggerName)
    {
        animator.SetTrigger(TriggerName);

    }

    public IEnumerator PlayRandomCatAnimation()
    {
        yield return new WaitForSeconds(2);
        RandomAnimationIntival = Random.Range(0, AnimationTriggers.Length);
        animator.SetTrigger(AnimationTriggers[RandomAnimationIntival]);
        yield return new WaitForSeconds(1);
        StartCoroutine(PlayRandomCatAnimation());
    }

    public IEnumerator Blink()
    {
        BlinkIntival = Random.Range(2, 6);
        yield return new WaitForSeconds(BlinkIntival);
        TriggerAnimation(AnimationTriggers[2]);
        StartCoroutine(Blink());    



    }

   

}
