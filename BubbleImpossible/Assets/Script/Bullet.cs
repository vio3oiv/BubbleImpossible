using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 7f; // 탄 속도
    public int damage = 1; // 기본 피해량
    public bool isEnemyBullet = false; // 🟢 적(특히 SpecialBird)의 탄인지 확인

    void Start()
    {
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.left * speed; // 탄을 왼쪽으로 발사
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isEnemyBullet && collision.CompareTag("Enemy")) // 플레이어 탄이 적을 맞춘 경우
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.hp -= damage;
            }
            Destroy(gameObject); // 탄 제거
        }
        else if (isEnemyBullet && collision.CompareTag("Player")) // SpecialBird의 탄이 플레이어를 맞춘 경우
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(damage); // 플레이어 HP 감소
            }
            Destroy(gameObject); // 탄 제거
        }
        else if (collision.CompareTag("Wall")) // 벽과 충돌 시 삭제
        {
            Destroy(gameObject);
        }
    }
}
