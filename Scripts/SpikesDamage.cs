using UnityEngine;

public class SpikesDamage : MonoBehaviour
{
    public int damage = 2; // Урон, который наносят колючки
    public float damageCooldown = 1f; // Время между получением урона (чтобы игрок не получал урон каждый кадр)
    private float lastDamageTime; // Время последнего нанесения урона
    private float lastDamageTimeEnemy;
    private AudioSource AudioSource;

    private void Start()
    {
        AudioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Проверяем, что коллизия произошла с игроком
        if (collision.CompareTag("Player"))
        {
            ApplyDamageToPlayer(collision.gameObject);
        }
        else if (collision.CompareTag("Enemy"))
        {
            ApplyDamageToEnemy(collision.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // Проверяем, что коллизия произошла с игроком и прошло достаточно времени с последнего урона
        if (collision.CompareTag("Player") && Time.time > lastDamageTime + damageCooldown)
        {
            ApplyDamageToPlayer(collision.gameObject);
        }
        else if (collision.CompareTag("Enemy") && Time.time > lastDamageTimeEnemy + damageCooldown)
        {
            ApplyDamageToEnemy(collision.gameObject);
        }
    }

    private void ApplyDamageToPlayer(GameObject player)
    {
        // Получаем компонент PlayerHealth у игрока
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            AudioSource.Play();
            playerHealth.TakeDamage(damage); // Наносим урон
            lastDamageTime = Time.time; // Обновляем время последнего нанесения урона
        }
    }

    private void ApplyDamageToEnemy(GameObject enemy)
    {
        // Получаем компонент PlayerHealth у игрока
        EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            Debug.Log("Колючки колят врага!");
            AudioSource.Play();
            enemyHealth.TakeDamage(damage); // Наносим урон
            lastDamageTimeEnemy = Time.time; // Обновляем время последнего нанесения урона
        }
    }
}
