using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.LightAnchor;

public class NewEnemyAI : MonoBehaviour
{
    public enum EnemyType { Weak, Normal, Strong, Boss, Iyu }
    [Header("Тип врага")]
    public EnemyType type;

    [Header("Логика преследования")]
    public Transform player;
    public float chaseSpeed = 6f;
    public float jumpForce = 2f;
    public LayerMask groundLayer;
    public LayerMask obstacleLayers;
    public LayerMask wallsLayer;
    public float detectionRadius = 5f; // Радиус обнаружения игрока
    public bool drawDetectionRadius = true; // Визуализация радиуса в редакторе

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool shouldJump;
    public bool isChasing = false; // Флаг преследования
    private float offset = 0.5f;

    private Collider2D platformCollider;
    private float _pathfindingTime;
    private float _MAXpathfindingTime = 8f;
    private Vector2 bestCorner = Vector2.zero;
    private bool hasJumped;
    private bool findpath_isGrounded;
    private bool shouldFindPath = false;
    private Vector3 lastPlayerPosition;

    [Header("Логика патрулирования")]
    public float patrolSpeed = 1f;
    public float pauseDuration = 2f; // Длительность паузы между разворотами
    public Transform patrolStartPoint; // Начальная точка патрулирования
    public Transform patrolEndPoint; // Конечная точка патрулирования
    private float pauseTimer = 0f; // Таймер для паузы между разворотами
    private bool movingToEnd = true;

    [Header("Поворот персонажа")]
    public bool facingRight = true;
    private bool isKnockbacked = false;

    Animator animator;
    public SoundManager soundManager;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        soundManager = GetComponent<SoundManager>();



        switch (type)
        {
            case EnemyType.Weak:
                detectionRadius = 10f;
                break;
            case EnemyType.Strong:
                detectionRadius = 15f;
                break;
            case EnemyType.Boss:
                detectionRadius = 25f;
                chaseSpeed = 3f;
                break;
            case EnemyType.Iyu:
                detectionRadius = 500f;
                break;
            default: // Normal
                detectionRadius = 13f;
                break;
        }

        if (type == EnemyType.Iyu)
        {
            gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (isKnockbacked) return;

        HandleFacingDirection();
        CheckGrounded();

        // Проверяем расстояние до игрока и видимость ТОЛЬКО если не в режиме поиска пути
        if (!shouldFindPath)
        {
            HandlePlayerDetection();
        }

        // Если в режиме поиска пути - игнорируем игрока полностью
        if (shouldFindPath)
        {
            HandlePathfinding();
            return;
        }
        else
        {
            _pathfindingTime = 0;
        }

        // Дальнейший код выполняется только если isChasing = true
        if (isChasing)
        {
            HandleChasing();
        }
    }

    private void HandleChasing()
    {
        float direction = Mathf.Sign(player.position.x - transform.position.x);

        // Проверка, находится ли игрок выше (желтый луч вверх)
        bool isPlayerAbove = Physics2D.Raycast(transform.position, Vector2.up, detectionRadius, 1 << player.gameObject.layer);
        Debug.DrawRay(transform.position, Vector2.up * detectionRadius, isPlayerAbove ? Color.yellow : Color.white);
        // Проверка, находится ли игрок ниже под платформой
        bool isPlayerBelow = Physics2D.Raycast(transform.position + new Vector3(0, -GetComponent<Collider2D>().bounds.extents.y - 0.5f, 0), Vector2.down, 5f, 1 << player.gameObject.layer);
        Debug.DrawRay(transform.position + new Vector3(0, -GetComponent<Collider2D>().bounds.extents.y - 0.5f, 0), Vector2.down * 5f, isPlayerBelow ? Color.yellow : Color.white);

        if (isGrounded)
        {
            // Движение
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            rb.velocity = new Vector2(direction * chaseSpeed, rb.velocity.y);
            if (type == EnemyType.Iyu)
            {
                // Если игрок далеко - преследовать
                if (distanceToPlayer > 2f)
                {
                    rb.velocity = new Vector2(direction * chaseSpeed, rb.velocity.y);
                    animator.SetFloat("horizontalMove", Mathf.Abs(rb.velocity.x));
                }
                // Если игрок близко - остановиться
                else
                {
                    rb.velocity = new Vector2(0, rb.velocity.y);
                    animator.SetFloat("horizontalMove", 0);
                }
            }
            else
            {
                animator.SetFloat("horizontalMove", Mathf.Abs(rb.velocity.x));
            } 

            // Луч для проверки земли перед врагом (синий луч)
            RaycastHit2D groundInFront = Physics2D.Raycast(transform.position, new Vector2(direction, 0), 3f, groundLayer);
            Debug.DrawRay(transform.position, new Vector2(direction, 0) * 3f, groundInFront.collider ? Color.blue : Color.cyan);

            // Луч для проверки пропасти впереди (зеленый луч вниз со смещением)
            Vector3 gapCheckStart = transform.position + new Vector3(direction, 0, 0);
            RaycastHit2D gapAhead = Physics2D.Raycast(gapCheckStart, Vector2.down, 3f, groundLayer);
            Debug.DrawRay(gapCheckStart, Vector2.down * 3f, gapAhead.collider ? Color.green : Color.magenta);

            // Луч для проверки платформы выше (белый луч вверх)
            RaycastHit2D platformAbove = Physics2D.Raycast(transform.position, Vector2.up, detectionRadius, groundLayer);
            Debug.DrawRay(transform.position, Vector2.up * detectionRadius, platformAbove.collider ? Color.white : Color.gray);

            if (!groundInFront.collider && !gapAhead.collider)
            {
                shouldJump = true;
            }
            else if (isPlayerAbove && platformAbove.collider)
            {
                shouldJump = true;
            }
            else if (isPlayerBelow)
            {
                shouldFindPath = true;
                platformCollider = gapAhead.collider;
                lastPlayerPosition = player.transform.position;
                isChasing = false;
            }
        }
    }

    private void HandleFacingDirection()
    {
        if (!facingRight && rb.velocity.x > 0)
        {
            Flip();
        }
        else if (facingRight && rb.velocity.x < 0)
        {
            Flip();
        }
    }

    private void CheckGrounded()
    {
        float currentDirection = facingRight ? 1 : -1;
        isGrounded = Physics2D.Raycast(transform.position + new Vector3(offset * currentDirection, 0, 0), Vector2.down, 1.7f, groundLayer);
        Debug.DrawRay(transform.position + new Vector3(offset * currentDirection, 0, 0), Vector2.down * 1.7f, isGrounded ? Color.green : Color.red);
    }

    private void HandlePlayerDetection()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        bool canSeePlayer = distanceToPlayer <= detectionRadius && CanSeePlayer();

        if (canSeePlayer)
        {
            isChasing = true;
        }
        else
        {
            isChasing = false;
            if (isGrounded)
            {
                Patrol();
            }
        }
    }

    private void HandlePathfinding()
    {
        _pathfindingTime += Time.deltaTime;
        if (_pathfindingTime > _MAXpathfindingTime) // Макс. время поиска
        {
            shouldFindPath = false;
            _pathfindingTime = 0;
        }
        else
        {
            FindPath();
        }
    }

    private void FixedUpdate()
    {
        if (isGrounded && shouldJump && isChasing)
        {
            Vector2 direction = Vector2.up;
            direction = (player.position - transform.position).normalized;
            shouldJump = false;
            Vector2 jumpDirection = direction * jumpForce;
            rb.AddForce(new Vector2(jumpDirection.x, jumpForce), ForceMode2D.Impulse);
            animator.SetFloat("verticalMove", rb.velocity.y);
            soundManager.PlaySound(1);
        }

    }

    // Проверка видимости игрока (нет препятствий между врагом и игроком)
    private bool CanSeePlayer()
    {
        Vector2 directionToPlayer = player.position - transform.position;

        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            directionToPlayer.normalized,
            detectionRadius,
            obstacleLayers);

        return hit.collider == null || hit.collider.transform == player;
    }

    void Patrol()
    {
        // Если враг на паузе, ждём
        if (pauseTimer > 0)
        {
            pauseTimer -= Time.deltaTime;
            return;
        }

        // Определяем текущую цель патрулирования
        Vector3 targetPoint = movingToEnd ? patrolEndPoint.position : patrolStartPoint.position;

        // Двигаемся к целевой точке
        Vector3 direction = (targetPoint - transform.position).normalized;
        direction.y = 0;
        direction.z = 0;

        // Применяем движение
        rb.velocity = new Vector2(direction.x * patrolSpeed, rb.velocity.y);
        animator.SetFloat("horizontalMove", Mathf.Abs(rb.velocity.x));
        // Проверяем достижение точки с более гибким условием
        if (Vector3.Distance(transform.position, targetPoint) < 0.5f) // Увеличил радиус
        {
            movingToEnd = !movingToEnd;
            pauseTimer = pauseDuration;
            rb.velocity = Vector2.zero; // Останавливаем врага на паузе
        }
    }

    void FindPath()
    {
        if (bestCorner == Vector2.zero)
        {
            bestCorner = BestCorner();
            if (bestCorner == Vector2.zero)
            {
                shouldFindPath = false;
                return;
            }
        }
        
        // Направление к цели (нормализованное, чтобы скорость была постоянной)
        Vector2 direction = (bestCorner - (Vector2)transform.position).normalized;
        float raydir = facingRight ? 1 : -1;
        RaycastHit2D groundCheck = Physics2D.Raycast(transform.position + new Vector3(direction.x * 0.5f + raydir, 0, 0), Vector2.down, 3f, groundLayer);

        Debug.DrawRay(transform.position + new Vector3(direction.x * 0.5f + raydir, 0, 0), Vector2.down * 3f, groundCheck.collider ? Color.green : Color.red, 0.1f);

        // Обновляем флаг isGrounded
        findpath_isGrounded = groundCheck.collider != null;

        // Если враг на земле и ещё не прыгнул
        if (isGrounded && !hasJumped)
        {
            // Направление к цели
            rb.velocity = new Vector2(direction.x * chaseSpeed, rb.velocity.y);
            animator.SetFloat("horizontalMove", Mathf.Abs(rb.velocity.x));
            // Если достигли края платформы (нет земли впереди) — прыгаем
            if (!groundCheck.collider)
            {
                rb.AddForce(new Vector2(direction.x * jumpForce, jumpForce), ForceMode2D.Impulse);
                animator.SetFloat("verticalMove", rb.velocity.y);
                hasJumped = true; // Запрещаем прыжок до приземления
            }
        }
        // Если враг приземлился после прыжка
        else if (findpath_isGrounded && hasJumped)
        {
            shouldFindPath = false;
            bestCorner = Vector2.zero;
            //идёт к последней точке игрока
            hasJumped = false; // Сбрасываем флаг прыжка
            return;
        }
    }

    Vector2 BestCorner()
    {
        if (platformCollider == null)
            return Vector2.zero; // или другое значение по умолчанию

        Bounds platformBounds = platformCollider.bounds;
        Vector2 topLeft = new Vector2(platformBounds.min.x, platformBounds.max.y);
        Vector2 topRight = new Vector2(platformBounds.max.x, platformBounds.max.y);
        Vector2 enemyPos = rb.transform.position;

        // Определяем ближайший угол
        bool isLeftCloser = Vector2.Distance(enemyPos, topLeft) < Vector2.Distance(enemyPos, topRight);
        Vector2 closestCorner = isLeftCloser ? topLeft : topRight;
        Vector2 farthestCorner = isLeftCloser ? topRight : topLeft;

        // Проверяем, касается ли ближайший угол стены
        bool isClosestCornerBlocked = Physics2D.OverlapCircle(closestCorner, 0.05f, wallsLayer) != null; ;
        Debug.DrawLine(enemyPos, closestCorner, isClosestCornerBlocked ? Color.red : Color.green, 0.5f);
        
        // Если ближайший угол свободен — возвращаем его
        if (!isClosestCornerBlocked)
        {
            Debug.Log("Выбран ближайший угол (не заблокирован стеной)");
            return closestCorner;
        }

        // Проверяем дальний угол
        bool isFarthestCornerBlocked = Physics2D.OverlapCircle(farthestCorner, 0.05f, wallsLayer) != null; ;
        Debug.DrawLine(enemyPos, farthestCorner, isFarthestCornerBlocked ? Color.red : Color.yellow, 0.5f);
        // Если дальний угол свободен — возвращаем его
        if (!isFarthestCornerBlocked)
        {
            Debug.Log("Выбран дальний угол (ближайший заблокирован)");
            return farthestCorner;
        }

        // Оба угла заблокированы — выходим
        Debug.Log("Оба угла заблокированы стеной!");
        return Vector2.zero; // или другой сигнал о невозможности выбора

    }

    // Визуализация радиуса обнаружения в редакторе
    private void OnDrawGizmos()
    {
        if (drawDetectionRadius)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    public void Knockback(Vector2 direction, float force)
    {
        isKnockbacked = true;
        rb.velocity = Vector2.zero;
        rb.AddForce(direction.normalized * force, ForceMode2D.Impulse);
        Invoke(nameof(ResetKnockback), 0.3f); // Возвращаем управление через 0.3 сек
    }

    private void ResetKnockback()
    {
        isKnockbacked = false;
    }

    public void PlayFootstepsSound()
    {
        soundManager.PlaySound(0);
    }
}