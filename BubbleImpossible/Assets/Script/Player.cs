using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public int hp = 3;
    public float speed = 5f;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 movement;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float shootCooldown = 0.2f;
    public Collider2D moveBounds;
    private bool canShoot = true;
    private bool isDead = false;
    public GameObject[] balloonSprites; 
    public GameObject balloonPopEffectPrefab; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator.Play("PlayerIdle");
    }

    void Update()
    {
        if (isDead) return;

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
        if (isDead) return;

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
        if (isDead) return;

        hp -= damage;
        Debug.Log($"🚨 플레이어 HP: {hp}");

        // Animator에서 "PlayerHit" 상태로 전환 (트리거)
        animator.SetTrigger("HitTrigger");
        PopBalloon();

        if (hp <= 0)
        {
            // 사망 트리거
            animator.SetTrigger("DeathTrigger");
            // 사망 코루틴 직접 호출 (or Animation Event에서 호출 가능)
            StartCoroutine(Die());
        }
    }
    void PopBalloon()
    {
        // HP가 0 이하이면 더 이상 풍선이 없음
        if (hp < 0) return;

        // balloonSprites 배열에서 인덱스 = 현재 HP
        // 예) HP가 3 -> 2로 줄면 balloonSprites[2]를 제거
        // (배열 인덱스와 HP를 일치시키려면 배열 크기와 HP 최대치가 동일해야 함)

        if (hp < balloonSprites.Length)
        {
            GameObject balloon = balloonSprites[hp];
            if (balloon != null)
            {
                // 풍선 위치에서 폭발 이펙트 생성
                if (balloonPopEffectPrefab != null)
                {
                    Instantiate(balloonPopEffectPrefab, balloon.transform.position, Quaternion.identity);
                }
                // 풍선 제거
                Destroy(balloon);

                // 배열에서 참조도 없애서 중복 제거 방지
                balloonSprites[hp] = null;
            }
        }
    }

    IEnumerator Die()
    {
        isDead = true;

        Debug.Log("💀 플레이어 사망! 아래로 떨어짐");

        // 여기서 'Death' 애니메이션 재생은 Animator가 Trigger를 이용해 자동 전환
        // animator.Play("PlayerDeath");  // 필요하다면 직접 호출도 가능

        // 중력 적용 후 아래로 떨어지는 연출
        rb.gravityScale = 1f;
        rb.linearVelocity = new Vector2(0, -5f);

        yield return new WaitForSeconds(3f);

        // 3초 후 게임 오버
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
