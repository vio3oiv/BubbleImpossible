using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f; // 탄 속도
    public float delay = 0.1f; // 적과 충돌 후 삭제까지의 지연 시간
    public Sprite hitSprite; // 적과 충돌 시 변경할 스프라이트

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool hasHit = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb.freezeRotation = true;
        rb.linearVelocity = new Vector2(speed, 0f); // 오른쪽으로 이동

        // 🟢 Update에서 직접 transform.position을 변경하지 않음 (Rigidbody2D 사용)
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasHit && collision.CompareTag("Enemy"))
        {
            hasHit = true;
            spriteRenderer.sprite = hitSprite; // 스프라이트 변경
            rb.linearVelocity = Vector2.zero; // 속도 정지
            Destroy(gameObject, delay); // 지정된 시간 후 삭제
        }
    }
}
