using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    private bool isInRange = false;

    [SerializeField] private KeyCode interactKey = KeyCode.E;

    // uses Unity Events to communicate between scripts

    public UnityEvent interactAction; 
    public UnityEvent enterAction;
    public UnityEvent exitAction;

    void Update()
    {
        if(isInRange && Input.GetKeyDown(interactKey)) // If the player uses the interact key while in range, invoke the interaction specified
        {
            interactAction.Invoke();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) // uses player collision to check if player is in range
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInRange = true;
            enterAction.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D collision) // uses player collision to check if player has exited range
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInRange = false;
            exitAction.Invoke();
        }
    }
}
