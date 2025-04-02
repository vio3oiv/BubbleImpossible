using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Boss : MonoBehaviour
{
    [Header("보스 기본 설정")]
    public int hp = 2;                // 보스 체력
    public bool isBoss = true;        // 보스 여부
    public string enemyName;
    public float bulletSpeed = 7f;    // 탄 속도
    public float fireRate = 3f;       // 탄 발사 간격

    // 보스 불렛 프리팹 리스트 (여러 개 중 랜덤 선택)
    public List<GameObject> bossBulletPrefabs;

    public GameObject explosionPrefab;    // 폭발 효과 프리팹

    [Header("보스 이동 설정")]
    public float moveDistance = 3f;   // 위아래 이동 범위
    public float moveSpeed = 2f;      // 이동 속도
    private Vector3 startPosition;
    private bool movingUp = true;

    [Header("보스 발사 설정")]
    // 여러 개의 발사 위치 중 랜덤 선택
    public Transform[] firePoint;

    private Animator animator;
    private Transform player;
    public bool isDying = false;

    void Start()
    {
        // 시작 위치 및 컴포넌트 초기화
        startPosition = transform.position;
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        // 보스라면 탄 발사 코루틴 시작
        if (isBoss)
        {
            StartCoroutine(FireRoutine());
        }
    }

    void Update()
    {
        // 보스 위아래 이동 처리
        MoveUpDown();
    }

    /// <summary>
    /// 보스가 fireRate 간격으로 탄을 발사하는 코루틴
    /// </summary>
    IEnumerator FireRoutine()
    {
        while (!isDying)
        {
            yield return new WaitForSeconds(fireRate);
            Fire();
        }
    }

    /// <summary>
    /// firePoint 배열 내에서 랜덤한 발사 위치를 선택하고,
    /// bossBulletPrefabs 리스트 내에서 랜덤한 탄 프리팹을 선택하여 탄을 발사합니다.
    /// </summary>
    void Fire()
    {
        // 보스 불렛 프리팹 리스트가 비어있으면 오류 출력
        if (bossBulletPrefabs == null || bossBulletPrefabs.Count == 0)
        {
            Debug.LogError("🚨 보스 불렛 프리팹 리스트가 비어 있습니다!");
            return;
        }
        // firePoint 배열이 할당되지 않았거나 비어있으면 오류 출력
        if (firePoint == null || firePoint.Length == 0)
        {
            Debug.LogError("🚨 firePoint 배열이 설정되지 않았습니다!");
            return;
        }

        // 랜덤 인덱스 선택: 탄 프리팹과 발사 위치
        int randomBulletIndex = Random.Range(0, bossBulletPrefabs.Count);
        GameObject selectedBulletPrefab = bossBulletPrefabs[randomBulletIndex];

        int randomFirePointIndex = Random.Range(0, firePoint.Length);
        Transform chosenFirePoint = firePoint[randomFirePointIndex];

        // 선택된 발사 위치에서 탄 인스턴스화
        GameObject bullet = Instantiate(selectedBulletPrefab, chosenFirePoint.position, Quaternion.identity);
        if (bullet == null)
        {
            Debug.LogError("🚨 보스 탄 생성 실패!");
            return;
        }

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("🚨 보스 탄에 Rigidbody2D가 없습니다!");
            return;
        }

        // 탄을 왼쪽 방향으로 발사 (필요 시 방향 수정)
        rb.linearVelocity = Vector2.left * bulletSpeed;
        Debug.Log($"🚀 보스가 탄을 발사했습니다! 속도: {rb.linearVelocity}");
    }

    /// <summary>
    /// 보스가 위아래로 이동합니다.
    /// </summary>
    void MoveUpDown()
    {
        if (movingUp)
        {
            transform.position += Vector3.up * moveSpeed * Time.deltaTime;
            if (transform.position.y >= startPosition.y + moveDistance)
            {
                transform.position = new Vector3(transform.position.x, startPosition.y + moveDistance, transform.position.z);
                movingUp = false;
            }
        }
        else
        {
            transform.position += Vector3.down * moveSpeed * Time.deltaTime;
            if (transform.position.y <= startPosition.y - moveDistance)
            {
                transform.position = new Vector3(transform.position.x, startPosition.y - moveDistance, transform.position.z);
                movingUp = true;
            }
        }
    }

    /// <summary>
    /// 충돌 처리:
    /// - 플레이어 탄과 충돌 시 보스 체력 감소 및 사망 처리
    /// - 플레이어와 충돌 시 플레이어에 데미지 전달
    /// </summary>
    void OnTriggerEnter2D(Collider2D collision)
    {
        // "BossBullet" 태그의 탄과 충돌 시
        if (collision.CompareTag("BossBullet"))
        {
            hp -= 1;
            Debug.Log($"🚨 보스 체력: {hp}");
            if (hp <= 0 && !isDying)
            {
                isDying = true;
                animator.SetTrigger("OnDeath");
                StartCoroutine(FlyUpAndDestroy());
            }
            Destroy(collision.gameObject);
        }
        // 플레이어와 충돌 시
        else if (collision.CompareTag("Player"))
        {
            Player playerScript = collision.GetComponent<Player>();
            if (playerScript != null)
            {
                if (!isDying)
                {
                    playerScript.TakeDamage(1);
                }
                else
                {
                    playerScript.ForceIdle();
                }
            }
        }
    }

    /// <summary>
    /// 사망 애니메이션 후 보스가 위로 날아오르며 파괴됩니다.
    /// </summary>
    IEnumerator FlyUpAndDestroy()
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
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
