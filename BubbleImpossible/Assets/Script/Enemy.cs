using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public int hp = 2; // 적 체력
    public float speed = 2f; // 기본 이동 속도
    public string enemyName; // 적 이름 (특수 적 구분)
    private Transform player;
    private Animator animator;
    //private bool hasPassedPlayer = false;
    public bool isDying = false; // 사망 여부

    // SpecialBird 관련 변수
    public bool isSpecialBird = false; // SpecialBird인지 여부
    public Vector2 targetPosition; // SpecialBird 이동 목표 위치
    private bool hasReachedTarget = false; // 목표 지점 도착 여부
    public GameObject specialBulletPrefab; // SpecialBird의 탄 프리팹 (Bullet과 다름)
    public Transform firePoint; // 탄 발사 위치
    public float bulletSpeed = 7f; // 탄 속도
    public float fireRate = 3f; // 탄 발사 간격
    

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        animator = GetComponent<Animator>();

        // SpecialBird이면 탄 발사 시작
        if (isSpecialBird)
        {
            StartCoroutine(FireRoutine());
        }

        FindFirstObjectByType<EnemyManager>()?.RegisterEnemy(this);
    }

    void Update()
    {
        if (!isDying)
        {
            if (isSpecialBird)
            {
                MoveToTarget();
            }
            else
            {
                MoveLeft();
            }
        }
    }

    void MoveLeft()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;
    }

    void MoveToTarget()
    {
        if (!hasReachedTarget)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
            {
                hasReachedTarget = true;
            }
        }
    }

    IEnumerator FireRoutine()
    {
        while (true)
        {
            Debug.Log("🔥 SpecialBird가 탄을 발사하려 합니다...");
            yield return new WaitForSeconds(fireRate);
            Fire();
        }
    }


    void Fire()
    {
        if (specialBulletPrefab == null)
        {
            Debug.LogError("🚨 SpecialBird의 탄 프리팹이 설정되지 않았습니다!");
            return;
        }

        if (firePoint == null)
        {
            Debug.LogError("🚨 SpecialBird의 firePoint가 설정되지 않았습니다!");
            return;
        }

        GameObject bullet = Instantiate(specialBulletPrefab, firePoint.position, Quaternion.identity);

        if (bullet == null)
        {
            Debug.LogError("🚨 SpecialBird 탄이 생성되지 않았습니다!");
            return;
        }

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogError("🚨 SpecialBird 탄에 Rigidbody2D가 없습니다!");
            return;
        }

        rb.linearVelocity = Vector2.left * bulletSpeed; // 탄을 왼쪽으로 발사
        Debug.Log($"🚀 SpecialBird가 탄을 발사했습니다! 속도: {rb.linearVelocity}");
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어 탄과 충돌 → 적 HP 감소
        if (collision.CompareTag("Bullet"))
        {
            hp -= 1;

            if (hp <= 0 && !isDying)
            {
                isDying = true;
                animator.SetTrigger("OnDeath");

                EnemyManager enemyManager = FindFirstObjectByType<EnemyManager>();
                if (enemyManager != null)
                {
                    StartCoroutine(FlyUpAndDestroy(enemyManager));
                }
            }
            Destroy(collision.gameObject);
        }
        // 플레이어와 충돌
        else if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                // 에너미가 아직 사망 중이 아니면 → 플레이어 HP 감소
                if (!isDying)
                {
                    player.TakeDamage(1);
                }
                else
                {
                    // 사망 애니메이션 중이라면 → 플레이어를 강제로 Idle 상태로 전환
                    player.ForceIdle();
                }
            }
        }
    }



    private IEnumerator FlyUpAndDestroy(EnemyManager enemyManager)
    {
        float flySpeed = 2f;
        float duration = 1f;
        float timer = 0f;

        while (timer < duration)
        {
            transform.position += Vector3.up * flySpeed * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }

        if (enemyManager.explosionPrefab != null)
        {
            Instantiate(enemyManager.explosionPrefab, transform.position, Quaternion.identity);
        }

        enemyManager.enemies.Remove(this);
        Destroy(gameObject);
    }
}