using UnityEngine;

public class CamMove : MonoBehaviour
{
    public GameObject player;

    void Update()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 0.5f, -10f);
    }
}

