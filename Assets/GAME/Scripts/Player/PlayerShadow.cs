using UnityEngine;

public class PlayerShadow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float Yoffset;

    void Update()
    {
        gameObject.transform.position = new Vector3(player.position.x, Yoffset, player.position.z);
    }
}
