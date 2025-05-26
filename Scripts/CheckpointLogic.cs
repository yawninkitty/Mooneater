using UnityEngine;

public class CheckpointLogic : MonoBehaviour
{
    public Transform player;
    public int index;
    private AudioSource audioSource;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;

        if (DataContainer.checkpointIndex == index)
        {
            player.position = transform.position;
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (DataContainer.checkpointIndex < index)
            {
                audioSource.Play();
            }
            
            // ≈сли индекс изменилс€ - сохран€ем убитых
            DataContainer.UpdateCheckpoint(index);
        }
    }
}