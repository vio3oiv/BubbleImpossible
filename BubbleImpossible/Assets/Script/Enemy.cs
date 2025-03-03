using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public int hp = 2;
    public float speed = 2f;
    public string enemyName;
    private Transform player;
    private Animator animator;
    private bool hasPassedPlayer = false;
    private bool isDying = false; // 적이 죽는 중인지 체크

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        animator = GetComponent<Animator>();

        FindFirstObjectByType<EnemyManager>()?.RegisterEnemy(this);
    }

    void Update()
    {
        if (!isDying) // 사망 중이 아닐 때만 이동
        {
            if (enemyName == "HomingBird" && player != null && !hasPassedPlayer)
            {
                if (transform.position.x > player.position.x)
                {
                    transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
                }
                else
                {
                    hasPassedPlayer = true;
                }
            }
            else
            {
                transform.position += Vector3.left * speed * Time.deltaTime;
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
                isDying = true; // 사망 중 플래그 설정
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
        float duration = 1f; // 1초 동안 떠오름
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
