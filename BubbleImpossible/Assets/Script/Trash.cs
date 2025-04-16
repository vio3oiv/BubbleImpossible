using UnityEngine;

public class Trash : MonoBehaviour
{
    // 플레이어에게 줄 피해량
    public int damage = 1;

    // 쓰레기가 떨어지는 속도를 조절하는 변수 (단위: m/s)
    public float fallSpeed = 5f;

    private Rigidbody2D rb;

    void Start()
    {
        // Rigidbody2D 컴포넌트를 가져와서, 중력 대신 일정한 속도로 떨어지도록 설정
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // 중력의 영향을 받지 않도록 설정하고, 일정한 속도로 아래 방향 이동하도록 속도 지정
            rb.gravityScale = 0;
            rb.linearVelocity = new Vector2(0, -fallSpeed);
        }
    }

    // 3D 환경인 경우 OnTriggerEnter(Collider other) 사용 (2D 환경이면 OnTriggerEnter2D로 변경)
    void OnTriggerEnter2D(Collider2D other)
    {
        // 충돌한 객체가 "Player" 태그를 가지고 있는지 확인
        if (other.CompareTag("Player"))
        {
            // Player 스크립트를 가져와서 HP 감소 처리
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
            // 충돌 후 쓰레기 오브젝트 제거
            Destroy(gameObject);
        }
    }
}
