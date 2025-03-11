using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public int hp = 3;          // 플레이어 HP
    public float speed = 5f;    // 이동 속도
    private bool isDead = false;
    private bool canShoot = true;
    private bool isInvulnerable = false; // 1초 무적 상태 여부

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    public Transform firePoint;
    public GameObject bulletPrefab;
    public Collider2D moveBounds;

    public float shootCooldown = 0.2f;
    public Sprite deathSprite;
    public Sprite damageSprite;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        animator.Play("PlayerIdle");
    }

    void Update()
    {
        if (isDead) return;

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        Vector2 movement = new Vector2(moveX, moveY).normalized;

        bool isMoving = movement != Vector2.zero;
        animator.SetBool("isMoving", isMoving);

        rb.linearVelocity = movement * speed;

        if (Input.GetKeyDown(KeyCode.Z) && canShoot)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (firePoint == null)
        {
            Debug.LogError("🚨 firePoint가 설정되지 않았습니다!");
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
        // 무적 상태라면 추가 데미지 무시
        if (isInvulnerable) return;

        hp -= damage;
        Debug.Log($"🚨 플레이어 HP: {hp}");

        // 1초 동안 무적
        StartCoroutine(InvulnerabilityRoutine(1f));

        // 피격 애니메이션 트리거
        animator.SetTrigger("HitTrigger");

        // HP가 0 이하 → 사망 로직
        if (hp <= 0)
        {
            animator.SetTrigger("DeathTrigger");
            StartCoroutine(Die());
        }
    }

    IEnumerator InvulnerabilityRoutine(float duration)
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(duration);
        isInvulnerable = false;
    }

    IEnumerator Die()
    {
        isDead = true;

        Debug.Log("💀 플레이어 사망! 아래로 떨어짐");

        // 사망 시 연출 (애니메이션, 중력 등)
        animator.enabled = false;
        rb.gravityScale = 1f;
        rb.linearVelocity = new Vector2(0, -5f);

        yield return new WaitForSeconds(3f);
        GameManager.instance.GameOver();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            TakeDamage(1);
        }
    }
}
