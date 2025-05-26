using UnityEngine;
using static NewEnemyAI;

public class EnemyAttack : MonoBehaviour
{
    [Header("��������� �����")]
    public float attackRange; // ��������� �����
    public int attackDamage; // ���� �� �����
    public float attackCooldown = 2f; // ����� ����� �������
    public LayerMask playerLayer; // ����, �� ������� ��������� �����
    public float knockbackForce = 2f; // ������������ ������ ��� �����

    private bool facingRight; // ����������� �����
    private NewEnemyAI enemyAI; // ������ �� ������ ���������� ������
    private BossSuperAttack bossSuperAttack;
    private float lastAttackTime = 0f; // ����� ��������� �����

    [Header("���������")]
    public float stunChance = 0.2f; // 20% ����
    public float stunDuration = 1f;

    [Header("���������������� �����")]
    public float bossSuperChance = 0.2f; // 20% ����

    public Animator animator;
    public SoundManager soundManager;

    private void Start()
    {   
        // �������� ������ �� EnemyAI
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
        // ��������� ����������� ����� ������ ����
        if (enemyAI != null)
        {
            facingRight = enemyAI.facingRight;
        }

        // ���������, ����� �� ���������
        if (Time.time > lastAttackTime + attackCooldown)
        {
            CheckForPlayerAndAttack();
        }
    }

    public void CheckForPlayerAndAttack()
    {
        // ���������� ����������� �����
        Vector2 attackDirection = facingRight ? transform.right : -transform.right;

        // ���������� Raycast ��� �������� ���������� ������ � ���� �����
        RaycastHit2D hit = Physics2D.Raycast(transform.position, attackDirection, attackRange, playerLayer);

        if (hit.collider != null)
        {
            // ���� ����� ���������, �������
            animator.SetBool("isAttacking", true);
            soundManager.PlaySound(2);
            Attack();
            lastAttackTime = Time.time; // ��������� ����� ��������� �����
        }
    }

    private void Attack()
    {
        
        // ���������� ����� ����� � ����������� �� ����������� �����
        Vector2 attackDirection = facingRight ? transform.right : -transform.right;
        Vector2 attackPoint = (Vector2)transform.position + attackDirection * attackRange;

        // ���������� ����� ��� ������������ ���� �����
        Debug.DrawLine(transform.position, attackPoint, Color.yellow, 1f);

        // ���������� Raycast ��� �������� ���������
        RaycastHit2D hit = Physics2D.Raycast(transform.position, attackDirection, attackRange, playerLayer);

        if (hit.collider != null)
        {
            PlayerHealth playerHealth = hit.collider.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                Debug.Log("������� ���� ������: " + hit.collider.name);
                playerHealth.TakeDamage(attackDamage);

                PlayerController PlayerController = hit.collider.GetComponent<PlayerController>();
                if (PlayerController != null)
                {
                    Debug.Log("������� ������!");
                    PlayerController.Knockback(attackDirection + Vector2.up * 0.3f, knockbackForce);
                }

                if ((enemyAI.type == EnemyType.Strong || enemyAI.type == EnemyType.Boss) && Random.value <= stunChance)
                {
                    PlayerStatusEffects playerEffects = hit.collider.gameObject.GetComponent<PlayerStatusEffects>();
                    if (playerEffects != null)
                    {
                        playerEffects.Stun(stunDuration);
                        Debug.Log("����� �������!");
                    }
                }
            }
        }
    }

    public void StopAnimation()
    {
        animator.SetBool("isAttacking", false);
    }

    // ������������ ���� ����� � ���������
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        // ���������� ����������� �����
        Vector2 attackDirection = facingRight ? transform.right : -transform.right;
        Vector2 attackPoint = (Vector2)transform.position + attackDirection * attackRange;

        // ������ ����� �����
        Gizmos.DrawLine(transform.position, attackPoint);

        // ������ ����� � ����� �����
        Gizmos.DrawWireSphere(attackPoint, 0.1f);
    }
}