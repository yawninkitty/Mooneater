using UnityEngine;
using System.Threading.Tasks;

public class PlayerAttack : MonoBehaviour
{
    [Header("Настройки атаки игрока")]
    public float attackRange = 2f; // Дистанция атаки
    public int attackDamage = 2; // Урон от атаки
    public LayerMask enemyLayer; // Слой, на котором находятся враги
    public float knockbackForce = 2f; // Отбрасывание врага при атаке
    public float attackCooldown = 2f;
    private float lastAttackTime = 0f;

    private bool facingRight; // Направление игрока
    private PlayerController playerController; // Ссылка на скрипт управления игроком
    public Animator animator;
    public SoundManager soundManager;

    private void Start()
    {
        // Получаем ссылку на PlayerController
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        soundManager = GetComponent<SoundManager>();

        if (playerController == null)
        {
            Debug.LogError("PlayerController не найден на этом объекте!");
        }
        PlayerBonusSystem.OnBonusesChanged += ApplyDamageBonus;
        ApplyDamageBonus(PlayerBonusSystem.Damage);
    }

    void Update()
    {
        // Обновляем направление игрока каждый кадр
        if (playerController != null)
        {
            facingRight = playerController.facingRight;
        }

    }

    public void ApplyDamageBonus(int Damage)
    {
         attackDamage = 2 + Damage;
    }

    void Attack()
    {
        // Определяем точку атаки в зависимости от направления игрока
        Vector2 attackDirection = facingRight ? transform.right : -transform.right;
        Vector2 attackPoint = (Vector2)transform.position + attackDirection * attackRange;

        // Отладочный вывод для визуализации зоны атаки
        Debug.DrawLine(transform.position, attackPoint, Color.red, 1f);

        // Используем Raycast для проверки попадания
        RaycastHit2D hit = Physics2D.Raycast(transform.position, attackDirection, attackRange, enemyLayer);
        
        if (hit.collider != null)
        {
            
            EnemyHealth enemyHealth = hit.collider.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                Debug.Log("Нанесен урон врагу: " + hit.collider.name);
                enemyHealth.TakeDamage(attackDamage);

                NewEnemyAI enemyAI = hit.collider.GetComponent<NewEnemyAI>();
                if (enemyAI != null)
                {
                    Debug.Log("Толкаем врага!");
                    enemyAI.Knockback(attackDirection + Vector2.up * 0.3f, knockbackForce);
                }
            }
        }
        else
        {
            Debug.Log("Враг не обнаружен в зоне атаки.");
        }
    }

    // Визуализация зоны атаки в редакторе
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        // Определяем направление атаки
        Vector2 attackDirection = facingRight ? transform.right : -transform.right;
        Vector2 attackPoint = (Vector2)transform.position + attackDirection * attackRange;

        // Рисуем линию атаки
        Gizmos.DrawLine(transform.position, attackPoint);

        // Рисуем сферу в точке атаки
        Gizmos.DrawWireSphere(attackPoint, 0.1f);
        
    }

    public void OnAttackButtonDown()
    {
        if (Time.time > lastAttackTime + attackCooldown)
        {
            animator.SetBool("isAttacking", true);
            soundManager.PlaySound(2);
            Attack();
            lastAttackTime = Time.time;
        }

    }

    public void StopAnimation()
    {
        animator.SetBool("isAttacking", false);
    }

}