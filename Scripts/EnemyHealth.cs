using UnityEngine;
using UnityEngine.UI;
using static NewEnemyAI;

public class EnemyHealth : MonoBehaviour
{
    [Header("Настройки здоровья")]
    public float maxHealth; // Максимальное здоровье врага
    public float currentHealth; // Текущее здоровье врага
    public GameObject healthBarUI; // Объект UI шкалы здоровья (Canvas с Slider)
    public Slider healthSlider; // Слайдер для отображения здоровья

    private NewEnemyAI enemyAI;
    private bool isHealthBarVisible = false; // Флаг, указывающий, видна ли шкала здоровья

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

        currentHealth = maxHealth; // Устанавливаем начальное здоровье
        if (healthBarUI != null)
        {
            healthBarUI.SetActive(false); // Скрываем шкалу здоровья при старте
        }
    }

    public void TakeDamage(float damage)
    {
        soundManager.PlaySound(3);
        if (!isHealthBarVisible)
        {
            
            // Показываем шкалу здоровья после первого получения урона
            healthBarUI.SetActive(true);
            isHealthBarVisible = true;
        }
        Debug.Log("Урон врагом получен");
        currentHealth -= damage; // Уменьшаем здоровье
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Ограничиваем здоровье в пределах от 0 до maxHealth
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die(); // Если здоровье закончилось, враг умирает
        }
    }

    private void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth / maxHealth; // Обновляем значение Slider (нормализованное значение)
        }
    }

    private void Die()
    {
        soundManager.PlaySound(4);
        Debug.Log("Враг умер!");
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
