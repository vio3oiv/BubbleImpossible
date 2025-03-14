using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public int hp = 3;
    public float speed = 5f;
    private bool isDead = false;
    private bool canShoot = true;
    private bool isInvulnerable = false;

    private Rigidbody2D rb;
    private Animator animator;

    public Transform firePoint;
    public GameObject bulletPrefab;
    public Collider2D moveBounds;

    public float shootCooldown = 0.2f;

    [Header("풍선(HP) 관련")]
    public GameObject[] balloonObjects;
    public GameObject balloonPopEffect;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // 시작 시 Idle 애니메이션
        animator.Play("PlayerIdle");
    }

    void Update()
    {
        // 사망하면 조작 불가
        if (isDead) return;

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        Vector2 movement = new Vector2(moveX, moveY).normalized;

        bool isMoving = (movement != Vector2.zero);
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
        // 무적 상태면 피해 무시
        if (isInvulnerable) return;

        hp -= damage;
        Debug.Log($"🚨 플레이어 HP: {hp}");

        // 풍선(HP) 제거
        PopBalloon();

        // 1초 무적
        StartCoroutine(InvulnerabilityRoutine(1f));

        // HP가 0 초과일 때만 HitTrigger (중간 피격)
        if (hp > 0)
        {
            animator.SetTrigger("HitTrigger");
        }

        // HP 0 이하 → 즉시 사망 애니메이션
        if (hp <= 0 && !isDead)
        {
            isDead = true;
            animator.SetTrigger("DeathTrigger"); // 사망 애니메이션 즉시 실행

            // 사망 처리 코루틴
            StartCoroutine(Die());
        }
    }

    void PopBalloon()
    {
        if (hp < 0) return;
        if (hp >= balloonObjects.Length) return;

        GameObject balloon = balloonObjects[hp];
        if (balloon != null)
        {
            if (balloonPopEffect != null)
            {
                Instantiate(balloonPopEffect, balloon.transform.position, Quaternion.identity);
            }
            Destroy(balloon);
            balloonObjects[hp] = null;
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
        Debug.Log("💀 플레이어 사망! 즉시 사망 애니메이션");

        // 여기서 이동/조작을 막기 위해서 isDead = true;
        // 사망 애니메이션 길이에 맞춰 잠깐 대기 (예: 1초)
        yield return new WaitForSeconds(1f);

        // 이후 아래로 떨어지는 연출을 할 수도 있고,
        // 바로 GameOver 처리를 할 수도 있음
        rb.gravityScale = 1f;
        rb.linearVelocity = new Vector2(0, -5f);

        GameManager.instance.GameOver();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            TakeDamage(1);
        }
    }

    public void ForceIdle()
    {
        // 플레이어를 강제로 Idle 애니메이션으로 전환
        animator.Play("PlayerIdle");

    }

}
