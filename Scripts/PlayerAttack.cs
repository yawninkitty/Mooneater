using UnityEngine;
using System.Threading.Tasks;

public class PlayerAttack : MonoBehaviour
{
    [Header("��������� ����� ������")]
    public float attackRange = 2f; // ��������� �����
    public int attackDamage = 2; // ���� �� �����
    public LayerMask enemyLayer; // ����, �� ������� ��������� �����
    public float knockbackForce = 2f; // ������������ ����� ��� �����
    public float attackCooldown = 2f;
    private float lastAttackTime = 0f;

    private bool facingRight; // ����������� ������
    private PlayerController playerController; // ������ �� ������ ���������� �������
    public Animator animator;
    public SoundManager soundManager;

    private void Start()
    {
        // �������� ������ �� PlayerController
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        soundManager = GetComponent<SoundManager>();

        if (playerController == null)
        {
            Debug.LogError("PlayerController �� ������ �� ���� �������!");
        }
        PlayerBonusSystem.OnBonusesChanged += ApplyDamageBonus;
        ApplyDamageBonus(PlayerBonusSystem.Damage);
    }

    void Update()
    {
        // ��������� ����������� ������ ������ ����
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
        // ���������� ����� ����� � ����������� �� ����������� ������
        Vector2 attackDirection = facingRight ? transform.right : -transform.right;
        Vector2 attackPoint = (Vector2)transform.position + attackDirection * attackRange;

        // ���������� ����� ��� ������������ ���� �����
        Debug.DrawLine(transform.position, attackPoint, Color.red, 1f);

        // ���������� Raycast ��� �������� ���������
        RaycastHit2D hit = Physics2D.Raycast(transform.position, attackDirection, attackRange, enemyLayer);
        
        if (hit.collider != null)
        {
            
            EnemyHealth enemyHealth = hit.collider.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                Debug.Log("������� ���� �����: " + hit.collider.name);
                enemyHealth.TakeDamage(attackDamage);

                NewEnemyAI enemyAI = hit.collider.GetComponent<NewEnemyAI>();
                if (enemyAI != null)
                {
                    Debug.Log("������� �����!");
                    enemyAI.Knockback(attackDirection + Vector2.up * 0.3f, knockbackForce);
                }
            }
        }
        else
        {
            Debug.Log("���� �� ��������� � ���� �����.");
        }
    }

    // ������������ ���� ����� � ���������
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        // ���������� ����������� �����
        Vector2 attackDirection = facingRight ? transform.right : -transform.right;
        Vector2 attackPoint = (Vector2)transform.position + attackDirection * attackRange;

        // ������ ����� �����
        Gizmos.DrawLine(transform.position, attackPoint);

        // ������ ����� � ����� �����
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