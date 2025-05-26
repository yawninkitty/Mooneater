using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("��������� �������� ������")]
    public Slider healthSlider; // ������ �� UI Slider
    public float maxHealth = 20f; // ������������ ��������
    private float currentHealth = 20f; // ������� ��������
    private ShopItem ShopItem;
    public SoundManager soundManager;

    void Start()
    {
        soundManager = GetComponent<SoundManager>();
        currentHealth = maxHealth; // ������������� ��������� ��������
        UpdateHealthUI(); // ��������� UI
        PlayerBonusSystem.OnBonusesChanged += ApplyHealthBonus;
        ApplyHealthBonus(PlayerBonusSystem.Health);
        CurrencyManager.ResetToCheckpointState();
        PlayerBonusSystem.ResetToCheckpointState();
        
    }

    public void ApplyHealthBonus(int Health)
    {
        maxHealth = 20 + PlayerBonusSystem.Health;
        Heal(maxHealth);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // ������������ �������� � �������� �� 0 �� maxHealth
        UpdateHealthUI();
        soundManager.PlaySound(3);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // ������������ �������� � �������� �� 0 �� maxHealth
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            if (healthSlider.value < currentHealth)
            {
                soundManager.PlaySound(5);
            }
            healthSlider.value = currentHealth; // ��������� �������� Slider
        }
    }

    private void Die()
    {

        // ��������� ���������� (���� ���� Rigidbody)
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;
        soundManager.PlaySound(6);
        // ������������� ����� ����� 1 �������
        Invoke("ReloadScene", 1f);
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}