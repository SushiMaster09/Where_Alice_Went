using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatterDoor : MonoBehaviour, ITalkable
{
    [SerializeField] private DialogueText dialogueText;
    [SerializeField] private DialogueController dialogueController;

    public void Talk(DialogueText dialogueText)
    {
        dialogueController.DisplayNextParagraph(dialogueText);
    }
}
