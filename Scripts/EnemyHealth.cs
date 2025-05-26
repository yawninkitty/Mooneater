using UnityEngine;
using UnityEngine.UI;
using static NewEnemyAI;

public class EnemyHealth : MonoBehaviour
{
    [Header("��������� ��������")]
    public float maxHealth; // ������������ �������� �����
    public float currentHealth; // ������� �������� �����
    public GameObject healthBarUI; // ������ UI ����� �������� (Canvas � Slider)
    public Slider healthSlider; // ������� ��� ����������� ��������

    private NewEnemyAI enemyAI;
    private bool isHealthBarVisible = false; // ����, �����������, ����� �� ����� ��������

    private CoinDropSystem coinDropSystem;

    public string enemyID;
    public FinishManager finishManager;

    public SoundManager soundManager;

    public GameObject Iyu;

    void Start()
    {   
        soundManager = GetComponent<SoundManager>();
        enemyID = gameObject.name;
        if (DataContainer.killedEnemies.Contains(enemyID))
        {
            gameObject.SetActive(false);
        }
        else
        {   
            DataContainer.pendingKilledEnemies.Clear();
            gameObject.SetActive(true);
        }

        coinDropSystem = GetComponent<CoinDropSystem>();
        enemyAI = GetComponent<NewEnemyAI>();
        switch (enemyAI.type)
        {
            case EnemyType.Weak:
                maxHealth = 6f;
                break;
            case EnemyType.Strong:
                maxHealth = 13f;
                break;
            case EnemyType.Boss:
                maxHealth = 56f;
                break;
            default: // Normal
                maxHealth = 9f;
                break;
        }

        currentHealth = maxHealth; // ������������� ��������� ��������
        if (healthBarUI != null)
        {
            healthBarUI.SetActive(false); // �������� ����� �������� ��� ������
        }
    }

    public void TakeDamage(float damage)
    {
        soundManager.PlaySound(3);
        if (!isHealthBarVisible)
        {
            
            // ���������� ����� �������� ����� ������� ��������� �����
            healthBarUI.SetActive(true);
            isHealthBarVisible = true;
        }
        Debug.Log("���� ������ �������");
        currentHealth -= damage; // ��������� ��������
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // ������������ �������� � �������� �� 0 �� maxHealth
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die(); // ���� �������� �����������, ���� �������
        }
    }

    private void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth / maxHealth; // ��������� �������� Slider (��������������� ��������)
        }
    }

    private void Die()
    {
        soundManager.PlaySound(4);
        Debug.Log("���� ����!");
        coinDropSystem.CoinDrop(enemyAI.type);
        if (!DataContainer.pendingKilledEnemies.Contains(enemyID))
        {
            DataContainer.pendingKilledEnemies.Add(enemyID);
        }
        if (enemyAI.type == EnemyType.Boss)
        {    
            finishManager.ActivateFinish();
            Iyu.gameObject.SetActive(true);
        }
        gameObject.SetActive(false);
        
    }
}
