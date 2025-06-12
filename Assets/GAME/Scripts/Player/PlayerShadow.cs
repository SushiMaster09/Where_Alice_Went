using UnityEngine;

public class PlayerShadow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float Yoffset;

    void Update()
    {
        if (player && gameObject)
            gameObject.transform.position = new Vector3(player.position.x, Yoffset, player.position.z);
    }
}
