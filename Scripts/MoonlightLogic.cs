using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class MoonlightLogic : MonoBehaviour
{
    private AudioSource AudioSource;
    public float healAmount;
    public string lightID;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        //чекпоинт логика
        lightID = gameObject.name;
        if (DataContainer.collectedMoonlight.Contains(lightID))
        {
            gameObject.SetActive(false);
        }
        else
        {
            DataContainer.pendingCollectedMoonlight.Clear();
            gameObject.SetActive(true);
        }


        AudioSource = GetComponent<AudioSource>();
    }

    private async void OnTriggerEnter2D(Collider2D collision)
    {
        // Проверяем, что коллизия произошла с игроком
        if (collision.CompareTag("Player"))
        {
            AudioSource.Play();
            ApplyHeal(collision.gameObject);
            await Task.Delay(700);
            if (!DataContainer.pendingCollectedMoonlight.Contains(lightID))
            {
                DataContainer.pendingCollectedMoonlight.Add(lightID);
            }
            gameObject.SetActive(false);
        }
    }

    private void ApplyHeal(GameObject player)
    {
        // Получаем компонент PlayerHealth у игрока
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            healAmount = playerHealth.maxHealth * 0.25f;
            playerHealth.Heal(healAmount);
        }
    }
}
