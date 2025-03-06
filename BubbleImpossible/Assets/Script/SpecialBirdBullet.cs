using UnityEngine;

public class SpecialBirdBullet : MonoBehaviour
{
    public float speed = 7f; // SpecialBird 탄 속도
    public int damage = 1; // 플레이어에게 입힐 피해량
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(-speed, 0f); // 왼쪽 방향으로 이동하도록 수정
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // 플레이어를 맞췄을 경우
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(damage); // 플레이어 HP 감소
            }
            Destroy(gameObject); // 탄 삭제
        }
        else if (collision.CompareTag("Boundary")) // 벽과 충돌 시 삭제
        {
            Destroy(gameObject);
        }
    }
}
