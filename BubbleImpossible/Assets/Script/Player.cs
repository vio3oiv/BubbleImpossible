using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public int hp = 3; // 플레이어 HP
    public float speed = 5f; // 이동 속도
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 movement;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float shootCooldown = 0.2f;
    public Collider2D moveBounds;
    private bool canShoot = true;
    private bool isDead = false;

    private SpriteRenderer spriteRenderer;
    public Sprite deathSprite; // 사망 시 변경할 스프라이트
    private Sprite defaultSprite;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            defaultSprite = spriteRenderer.sprite;
        }

        animator.Play("PlayerIdle"); // 시작 시 Idle 애니메이션 실행
    }

    void Update()
    {
        if (isDead) return; // 사망 시 조작 불가

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
        if (isDead) return; // 사망 시 이동 불가

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
        if (isDead) return;

        hp -= damage;
        Debug.Log($"🚨 플레이어 HP 감소: {damage}, 남은 HP: {hp}");

        if (hp <= 0)
        {
            StartCoroutine(Die()); // 사망 루틴 실행
        }
    }

    IEnumerator Die()
    {
        isDead = true;
        animator.enabled = false; // 애니메이션 정지

        if (deathSprite != null)
        {
            spriteRenderer.sprite = deathSprite;
        }

        Debug.Log("💀 플레이어 사망! 아래로 떨어짐");

        rb.gravityScale = 1f; // 중력 적용하여 아래로 떨어지도록 설정
        rb.linearVelocity = new Vector2(0, -5f); // 아래 방향으로 이동

        yield return new WaitForSeconds(3f); // 3초 대기

        GameManager.instance.GameOver(); // 3초 후 게임 오버 화면 표시
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            TakeDamage(1);
        }
    }
}
