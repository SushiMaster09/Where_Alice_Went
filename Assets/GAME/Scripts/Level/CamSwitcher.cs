using UnityEngine;

public class CamSwitcher : MonoBehaviour
{
    [SerializeField] GameObject VCam;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        VCam.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        VCam.SetActive(false);
    }
}
