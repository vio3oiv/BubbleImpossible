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
        // (2) 플레이어의 불렛이 보스 스페셜 탄환에 맞은 경우
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

        // (1) 플레이어와 충돌했을 때 (변환 전)
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                if (!isTransformed)
                {
                    // 변환 전: 플레이어 HP –1
                    player.TakeDamage(1);
                    Debug.Log("보스 스페셜 탄환이 플레이어에 맞아 HP가 감소했습니다.");
                    // 원한다면 탄환 제거 (혹은 계속 남길 수도 있음)
                    Destroy(gameObject);
                }
                else
                {
                    // (2) 변환 후: 플레이어의 애니메이션 변경 및 탄환 방향 전환
                    // (예시로 로그를 남기며 처리)
                    Debug.Log("변환된 보스 스페셜 탄환이 플레이어에 맞았습니다. 플레이어 애니메이션 변경 및 탄환 방향 전환.");
                    // 만약 Player 스크립트에 애니메이션 변경 함수가 있다면 호출할 수 있습니다.
                    // player.ChangeAnimationOnHit();

                    // 탄환의 이동 방향을 오른쪽으로 전환
                    direction = Vector2.right;
                    rb.linearVelocity = direction * speed;
                }
            }
        }

        // (3) 변환된 탄환이 보스와 충돌했을 때
        if (isTransformed && collision.CompareTag("Boss"))
        {
            Boss boss = collision.GetComponent<Boss>();
            if (boss != null)
            {
                boss.hp -= 1;  // 보스 HP –1
                Debug.Log("변환된 보스 스페셜 탄환이 보스에 맞아 HP가 감소했습니다.");
            }
            Destroy(gameObject);
        }
    }
}
