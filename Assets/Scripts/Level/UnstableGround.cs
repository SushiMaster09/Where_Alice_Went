using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnstableGround : MonoBehaviour
{
    [SerializeField] private float delay;

    [SerializeField] private GameObject ground;
    [SerializeField] private GameObject particles;
    private Animator groundAnimator;

    private void Start()
    {
        groundAnimator = ground.GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(CollapseFloor());
        }
    }

    public IEnumerator CollapseFloor()
    {
        groundAnimator.Play("Collapse");
        particles.SetActive(true);

        yield return new WaitForSeconds(delay);

        Destroy(ground);
    }
}
