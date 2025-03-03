using UnityEngine;

public class Player : MonoBehaviour
{
    public int hp = 3; // 플레이어 HP (기본값 3)
    public float speed = 5f; // 이동 속도
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 movement;
    public GameObject bulletPrefab; // 탄알 프리팹
    public Transform firePoint; // 탄이 발사될 위치
    public float shootCooldown = 0.2f; // 발사 후 쿨타임
    public Collider2D moveBounds; // 이동을 제한할 콜라이더
    private bool canShoot = true; // 발사 가능 여부

    private SpriteRenderer spriteRenderer;
    public Sprite hitSprite; // 피격 시 변경할 스프라이트
    public Sprite deathSprite; // 사망 시 변경할 스프라이트
    private Sprite defaultSprite; // 기본 스프라이트 저장

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            defaultSprite = spriteRenderer.sprite; // 기본 스프라이트 저장
        }

        if (firePoint == null)
        {
            Debug.LogError("🚨 firePoint가 설정되지 않았습니다! Unity 인스펙터에서 firePoint를 할당하세요.");
        }

        animator.Play("PlayerIdle"); // 시작 시 Idle 애니메이션 실행
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        movement = new Vector2(moveX, moveY).normalized;

        bool isMoving = movement != Vector2.zero;
        animator.SetBool("isMoving", isMoving);

        if (Input.GetKeyDown(KeyCode.Z) && canShoot)
        {
            Shoot();
        }
    }

    void FixedUpdate()
    {
        if (moveBounds != null)
        {
            Vector2 newPosition = rb.position;
            Bounds bounds = moveBounds.bounds;
            newPosition.x = Mathf.Clamp(newPosition.x, bounds.min.x, bounds.max.x);
            newPosition.y = Mathf.Clamp(newPosition.y, bounds.min.y, bounds.max.y);
            rb.position = newPosition;
        }

        rb.linearVelocity = movement * speed;
    }

    void Shoot()
    {
        if (firePoint == null)
        {
            Debug.LogError("🚨 firePoint가 설정되지 않았습니다! 탄을 발사할 수 없습니다.");
            return;
        }

        animator.SetTrigger("shootTrigger");
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        Invoke(nameof(ResetToIdle), shootCooldown);
        canShoot = false;
        Invoke(nameof(ResetShoot), shootCooldown);
    }

    void ResetShoot()
    {
        canShoot = true;
    }

    void ResetToIdle()
    {
        animator.Play("PlayerIdle");
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        Debug.Log($"🚨 플레이어 HP 감소: {damage}, 남은 HP: {hp}");

        if (hp > 0)
        {
            if (hitSprite != null)
            {
                spriteRenderer.sprite = hitSprite;
                Invoke(nameof(ResetSprite), 0.5f);
            }
        }
        else
        {
            if (deathSprite != null)
            {
                spriteRenderer.sprite = deathSprite;
                animator.enabled = false; // 애니메이션 중지
                Debug.Log("💀 플레이어 사망!");
            }
        }
    }

    void ResetSprite()
    {
        spriteRenderer.sprite = defaultSprite;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log($"⚠️ {collision.gameObject.name} (적)과 충돌함!");
            TakeDamage(1);
        }
    }
}
