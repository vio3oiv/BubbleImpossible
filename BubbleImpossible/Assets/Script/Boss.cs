using UnityEngine;

public class Boss : Enemy  // Boss가 Enemy를 상속받음
{
    [Header("보스 추가 설정")]
    public float moveDistance = 3f; // 위아래 이동 범위
    public float moveSpeed = 2f;    // 이동 속도

    private Vector3 startPosition;
    private bool movingUp = true;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        MoveUpDown();
    }

    void MoveUpDown()
    {
        if (movingUp)
        {
            transform.position += Vector3.up * moveSpeed * Time.deltaTime;
            if (transform.position.y >= startPosition.y + moveDistance)
            {
                transform.position = new Vector3(transform.position.x, startPosition.y + moveDistance, transform.position.z);
                movingUp = false;
            }
        }
        else
        {
            transform.position += Vector3.down * moveSpeed * Time.deltaTime;
            if (transform.position.y <= startPosition.y - moveDistance)
            {
                transform.position = new Vector3(transform.position.x, startPosition.y - moveDistance, transform.position.z);
                movingUp = true;
            }
        }
    }

    // Boss 고유의 기능(예: 불렛 발사 등)을 여기에 추가할 수 있습니다.
}
