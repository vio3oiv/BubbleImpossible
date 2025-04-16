using UnityEngine;

public class BossSpecialBullet : MonoBehaviour
{
    [Header("속도 및 이동")]
    public float speed = 5f;                  // 탄환 속도
    private Vector2 direction = Vector2.left; // 초기 이동 방향: 왼쪽

    [Header("스프라이트 설정")]
    public Sprite normalSprite;       // 초기(변환 전) 스프라이트
    public Sprite transformedSprite;  // 변환 후 스프라이트

    private bool isTransformed = false;  // 탄환이 변환되었는지 여부
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    public bool IsTransformed { get { return isTransformed; } }


    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        // 초기 스프라이트 설정 (원한다면)
        if (normalSprite != null)
        {
            spriteRenderer.sprite = normalSprite;
        }
        // 초기 이동 방향(왼쪽)으로 속도 설정
        rb.linearVelocity = direction * speed;
    }

    void Update()
    {
        // Rigidbody2D의 velocity를 사용하므로 별도 이동 코드는 필요 없음.
    }

    // 충돌 처리
    void OnTriggerEnter2D(Collider2D collision)
    {
        // 보스 컴포넌트를 직접 체크하여, 변환 전에는 보스에 영향을 주지 않도록 함
        if (!isTransformed && collision.GetComponent<Boss>() != null)
        {
            // 아직 변환 전이면 보스와 충돌해도 아무런 처리를 하지 않음
            return;
        }

        // (2) 플레이어의 불렛이 보스 스페셜 탄환에 맞은 경우 (아직 변환되지 않은 경우)
        if (!isTransformed && collision.CompareTag("Bullet"))
        {
            // 스프라이트를 변환하고 상태 플래그 업데이트
            if (transformedSprite != null)
            {
                spriteRenderer.sprite = transformedSprite;
                isTransformed = true;
                Debug.Log("보스 스페셜 탄환이 플레이어 불렛에 맞아 변환되었습니다.");
            }
            // 플레이어의 불렛은 제거
            Destroy(collision.gameObject);
            return;
        }

        // (1) 플레이어와 충돌했을 때
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                if (!isTransformed)
                {
                    // 변환 전: 플레이어 HP –1 후 탄환 제거
                    player.TakeDamage(1);
                    Debug.Log("보스 스페셜 탄환이 플레이어에 맞아 HP가 감소했습니다.");
                    Destroy(gameObject);
                }
                else
                {
                    // 변환 후: 플레이어 애니메이션 변경 및 탄환 방향 전환
                    Debug.Log("변환된 보스 스페셜 탄환이 플레이어에 맞았습니다. 플레이어 애니메이션 변경 및 탄환 방향 전환.");
                    player.ChangeAnimationOnHit();

                    // 탄환의 이동 방향을 오른쪽으로 전환
                    direction = Vector2.right;
                    rb.linearVelocity = direction * speed;
                }
            }
            return;
        }

        // (3) 변환된 탄환이 보스와 충돌했을 때
        if (isTransformed && collision.GetComponent<Boss>() != null)
        {
            Boss boss = collision.GetComponent<Boss>();
            if (boss != null)
            {
                boss.hp -= 1;  // 보스 HP –1
                Debug.Log("변환된 보스 스페셜 탄환이 보스에 맞아 HP가 감소했습니다.");
            }
            Destroy(gameObject);
            return;
        }
    }
}
