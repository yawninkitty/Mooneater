using UnityEngine;

public class SpikesDamage : MonoBehaviour
{
    public int damage = 2; // ����, ������� ������� �������
    public float damageCooldown = 1f; // ����� ����� ���������� ����� (����� ����� �� ������� ���� ������ ����)
    private float lastDamageTime; // ����� ���������� ��������� �����
    private float lastDamageTimeEnemy;
    private AudioSource AudioSource;

    private void Start()
    {
        AudioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ���������, ��� �������� ��������� � �������
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
        // ���������, ��� �������� ��������� � ������� � ������ ���������� ������� � ���������� �����
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
        // �������� ��������� PlayerHealth � ������
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            AudioSource.Play();
            playerHealth.TakeDamage(damage); // ������� ����
            lastDamageTime = Time.time; // ��������� ����� ���������� ��������� �����
        }
    }

    private void ApplyDamageToEnemy(GameObject enemy)
    {
        // �������� ��������� PlayerHealth � ������
        EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            Debug.Log("������� ����� �����!");
            AudioSource.Play();
            enemyHealth.TakeDamage(damage); // ������� ����
            lastDamageTimeEnemy = Time.time; // ��������� ����� ���������� ��������� �����
        }
    }
}
