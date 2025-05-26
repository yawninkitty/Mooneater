using UnityEngine;
using static NewEnemyAI;

public class EnemyAttack : MonoBehaviour
{
    [Header("Настройки атаки")]
    public float attackRange; // Дистанция атаки
    public int attackDamage; // Урон от атаки
    public float attackCooldown = 2f; // Время между атаками
    public LayerMask playerLayer; // Слой, на котором находится игрок
    public float knockbackForce = 2f; // Отбрасывание игрока при атаке

    private bool facingRight; // Направление врага
    private NewEnemyAI enemyAI; // Ссылка на скрипт управления врагом
    private BossSuperAttack bossSuperAttack;
    private float lastAttackTime = 0f; // Время последней атаки

    [Header("Оглушение")]
    public float stunChance = 0.2f; // 20% шанс
    public float stunDuration = 1f;

    [Header("Суперспособность босса")]
    public float bossSuperChance = 0.2f; // 20% шанс

    public Animator animator;
    public SoundManager soundManager;

    private void Start()
    {   
        // Получаем ссылку на EnemyAI
        enemyAI = GetComponent<NewEnemyAI>();
        animator = GetComponent<Animator>();
        soundManager = GetComponent<SoundManager>();

        switch (enemyAI.type)
        {
            case EnemyType.Weak:
                attackRange = 2f;
                attackDamage = 2;
                break;
            case EnemyType.Strong:
                attackRange = 4f;
                attackDamage = 3;
                break;
            case EnemyType.Boss:
                attackRange = 4f;
                attackDamage = 4;
                break;
            default: // Normal
                attackRange = 3f;
                attackDamage = 3;
                break;
        }
    }

    void Update()
    {
        // Обновляем направление врага каждый кадр
        if (enemyAI != null)
        {
            facingRight = enemyAI.facingRight;
        }

        // Проверяем, можно ли атаковать
        if (Time.time > lastAttackTime + attackCooldown)
        {
            CheckForPlayerAndAttack();
        }
    }

    public void CheckForPlayerAndAttack()
    {
        // Определяем направление атаки
        Vector2 attackDirection = facingRight ? transform.right : -transform.right;

        // Используем Raycast для проверки нахождения игрока в зоне атаки
        RaycastHit2D hit = Physics2D.Raycast(transform.position, attackDirection, attackRange, playerLayer);

        if (hit.collider != null)
        {
            // Если игрок обнаружен, атакуем
            animator.SetBool("isAttacking", true);
            soundManager.PlaySound(2);
            Attack();
            lastAttackTime = Time.time; // Обновляем время последней атаки
        }
    }

    private void Attack()
    {
        
        // Определяем точку атаки в зависимости от направления врага
        Vector2 attackDirection = facingRight ? transform.right : -transform.right;
        Vector2 attackPoint = (Vector2)transform.position + attackDirection * attackRange;

        // Отладочный вывод для визуализации зоны атаки
        Debug.DrawLine(transform.position, attackPoint, Color.yellow, 1f);

        // Используем Raycast для проверки попадания
        RaycastHit2D hit = Physics2D.Raycast(transform.position, attackDirection, attackRange, playerLayer);

        if (hit.collider != null)
        {
            PlayerHealth playerHealth = hit.collider.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                Debug.Log("Нанесен урон игроку: " + hit.collider.name);
                playerHealth.TakeDamage(attackDamage);

                PlayerController PlayerController = hit.collider.GetComponent<PlayerController>();
                if (PlayerController != null)
                {
                    Debug.Log("Толкаем игрока!");
                    PlayerController.Knockback(attackDirection + Vector2.up * 0.3f, knockbackForce);
                }

                if ((enemyAI.type == EnemyType.Strong || enemyAI.type == EnemyType.Boss) && Random.value <= stunChance)
                {
                    PlayerStatusEffects playerEffects = hit.collider.gameObject.GetComponent<PlayerStatusEffects>();
                    if (playerEffects != null)
                    {
                        playerEffects.Stun(stunDuration);
                        Debug.Log("Игрок оглушен!");
                    }
                }
            }
        }
    }

    public void StopAnimation()
    {
        animator.SetBool("isAttacking", false);
    }

    // Визуализация зоны атаки в редакторе
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        // Определяем направление атаки
        Vector2 attackDirection = facingRight ? transform.right : -transform.right;
        Vector2 attackPoint = (Vector2)transform.position + attackDirection * attackRange;

        // Рисуем линию атаки
        Gizmos.DrawLine(transform.position, attackPoint);

        // Рисуем сферу в точке атаки
        Gizmos.DrawWireSphere(attackPoint, 0.1f);
    }
}