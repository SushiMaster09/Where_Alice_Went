using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI talkingDialogueText;
    [SerializeField] private float typeSpeed = 10;

    private Queue<string> paragraphs = new Queue<string>();

    private bool convoEnded;
    private bool isTyping;

    private string p;

    private Coroutine typeDialogueCoroutine;

    public void DisplayNextParagraph(DialogueText dialogueText)
    {
        if (paragraphs.Count == 0)
        {
            if (!convoEnded)
            {
                //start a convo
                StartConversation(dialogueText);
            }
            else
            {
                //end the convo
                EndConversation();
                return;
            }
        }

        if (!isTyping)
        {
            p = paragraphs.Dequeue();
            typeDialogueCoroutine = StartCoroutine(TypeDialogueText(p));
        }

        //talkingDialogueText.text = p;

        if (paragraphs.Count == 0)
        {
            convoEnded = true;
        }
    }

    public void StartConversation(DialogueText dialogueText)
    {
        PlayerMovement.canMove = false;

        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }

        for (int i = 0; i < dialogueText.paragraphs.Length; i++)
        {
            paragraphs.Enqueue(dialogueText.paragraphs[i]);
        }
    }

    public void EndConversation()
    {
        PlayerMovement.canMove = true;

        paragraphs.Clear();
        convoEnded = false;

        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }

    private IEnumerator TypeDialogueText(string p)
    {
        float elapsedTime = 0f;

        int charIndex = 0;
        charIndex = Mathf.Clamp(charIndex, 0, p.Length);

        while (charIndex < p.Length)
        {
            elapsedTime += Time.deltaTime * typeSpeed;
            charIndex = Mathf.FloorToInt(elapsedTime);

            talkingDialogueText.text = p.Substring(0, charIndex);

            yield return null;
        }

        talkingDialogueText.text = p;
    }
}
