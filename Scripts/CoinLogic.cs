using UnityEngine;

public class CoinLogic : MonoBehaviour
{
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlaySound();
            CurrencyManager.AddCurrency(1);
            Destroy(gameObject);
        }
    }

    private void PlaySound()
    {
        audioSource.Play();
    }
}
