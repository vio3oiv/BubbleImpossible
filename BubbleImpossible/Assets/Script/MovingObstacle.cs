using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    [Header("위아래 이동 설정")]
    public float moveDistance = 2f;   // 이동할 최대 거리 (위아래 총합)
    public float moveSpeed = 2f;      // 이동 속도
    public Vector3 startPosition;     // 시작 위치 (Inspector에서 설정하거나, 코드에서 초기화)
    private bool goingUp = true;      // 위로 이동 중인지, 아래로 이동 중인지 체크

    void Start()
    {
        // 시작 위치를 코드에서 자동 설정하고 싶다면
        // startPosition = transform.position;
    }

    void Update()
    {
        MoveUpDown();
    }

    void MoveUpDown()
    {
        // 현재 위치
        Vector3 position = transform.position;

        if (goingUp)
        {
            // 위로 이동
            position.y += moveSpeed * Time.deltaTime;
            // 최대 거리만큼 이동했다면 방향 전환
            if (position.y >= startPosition.y + moveDistance)
            {
                position.y = startPosition.y + moveDistance;
                goingUp = false;
            }
        }
        else
        {
            // 아래로 이동
            position.y -= moveSpeed * Time.deltaTime;
            // 최대 거리만큼 아래로 이동했다면 방향 전환
            if (position.y <= startPosition.y - moveDistance)
            {
                position.y = startPosition.y - moveDistance;
                goingUp = true;
            }
        }

        transform.position = position;
    }

    // 플레이어와 충돌하면 HP를 깎음
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Player 스크립트에서 TakeDamage(1) 호출
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(1);
                Debug.Log($"🔻 플레이어가 장애물에 닿아 HP가 1 감소. 현재 HP: {player.hp}");
            }
        }
    }
}
