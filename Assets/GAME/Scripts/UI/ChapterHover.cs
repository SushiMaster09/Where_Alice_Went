using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ChapterHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string Contents = "Chapter Content";
    public TMP_Text TextBox;
    public void OnPointerEnter(PointerEventData eventData)
    {
        TextBox.text = Contents.ToLower();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TextBox.text = "";
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
