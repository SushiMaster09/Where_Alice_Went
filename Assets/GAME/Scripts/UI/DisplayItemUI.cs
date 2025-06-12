using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayItemUI : MonoBehaviour
{
    [SerializeField] private ItemData data;

    [SerializeField] private TextMeshProUGUI header;
    [SerializeField] private TextMeshProUGUI description;

    [SerializeField] private Image image;

    private bool displayStopped;

    private void Awake()
    {
        header.text = data.displayName;
        description.text = data.description;
        image.sprite = data.icon;
    }

    public void Display()
    {
        if (!displayStopped)
        {
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            displayStopped = false;

            if (gameObject.activeSelf)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
