using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public int hp = 2; // �� ü��
    public float speed = 2f; // �Ϲ� �� �̵� �ӵ�
    public string enemyName; // �� �̸� (Ư�� �� ����)
    private Transform player;
    private Animator animator;
    private bool hasPassedPlayer = false;
    private bool isDying = false; // ���� �״� ������ üũ

    // SpecialBird ����
    public bool isSpecialBird = false; // SpecialBird���� Ȯ��
    public Vector2 targetPosition; // SpecialBird�� �̵��� ��ǥ ��ġ
    private bool hasReachedTarget = false; // ��ǥ ���� ���� ����
    public GameObject bulletPrefab; // SpecialBird�� ź ������
    public Transform firePoint; // ź �߻� ��ġ
    public float bulletSpeed = 7f; // ź �ӵ�
    public float fireRate = 3f; // ź �߻� �ֱ�

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        animator = GetComponent<Animator>();

        // SpecialBird�̸� ź �߻� ��ƾ ����
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
            yield return new WaitForSeconds(fireRate);
            Fire();
        }
    }

    void Fire()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            Bullet bulletScript = bullet.GetComponent<Bullet>();

            if (bulletScript != null)
            {
                bulletScript.isEnemyBullet = true; // 🟢 SpecialBird의 탄으로 설정
            }

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.left * bulletSpeed; // 탄을 왼쪽으로 발사
            }
        }
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
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
