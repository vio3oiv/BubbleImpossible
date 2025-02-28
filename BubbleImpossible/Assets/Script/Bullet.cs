using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public Sprite hitSprite; // 충돌 시 변경될 스프라이트

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool hasHit = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        rb.freezeRotation = true; // 탄환이 회전하지 않도록 설정

        // transform.right이 아니라 직접 방향을 설정
        rb.linearVelocity = new Vector2(speed, 0f); // X축 기준 오른쪽으로 이동
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasHit && collision.gameObject.tag == "Enemy") // CompareTag 대신 직접 태그 비교
        {
            hasHit = true;
            spriteRenderer.sprite = hitSprite; // 충돌 시 스프라이트 변경
            rb.linearVelocity = Vector2.zero; // 탄환 멈추기
            Destroy(gameObject, 0.5f); // 0.5초 후 탄환 제거
        }
    }

}
