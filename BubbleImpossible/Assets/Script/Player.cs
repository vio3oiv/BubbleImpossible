using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f; // 이동 속도
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 movement;
    public GameObject bulletPrefab; // 탄알 프리팹
    public Transform firePoint; // 탄이 발사될 위치
    public float shootCooldown = 0.2f; // 발사 후 쿨타임
    public Collider2D moveBounds; // 이동을 제한할 콜라이더
    private bool canShoot = true; // 발사 가능 여부

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // firePoint가 설정되지 않았다면 경고 출력
        if (firePoint == null)
        {
            Debug.LogError("🚨 firePoint가 설정되지 않았습니다! Unity 인스펙터에서 firePoint를 할당하세요.");
        }

        // 시작 시 Idle 애니메이션 실행
        animator.Play("PlayerIdle");
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        movement = new Vector2(moveX, moveY).normalized;

        // 이동 애니메이션 설정
        bool isMoving = movement != Vector2.zero;
        animator.SetBool("isMoving", isMoving);

        // Z키를 누르면 발사
        if (Input.GetKeyDown(KeyCode.Z) && canShoot)
        {
            Shoot();
        }
    }

    void FixedUpdate()
    {
        // 이동 제한이 설정되어 있을 때 위치 보정
        if (moveBounds != null)
        {
            Vector2 newPosition = rb.position;
            Bounds bounds = moveBounds.bounds;
            newPosition.x = Mathf.Clamp(newPosition.x, bounds.min.x, bounds.max.x);
            newPosition.y = Mathf.Clamp(newPosition.y, bounds.min.y, bounds.max.y);
            rb.position = newPosition;
        }

        // Rigidbody2D의 linearVelocity를 사용한 이동 처리
        rb.linearVelocity = movement * speed;
    }

    void Shoot()
    {
        if (firePoint == null)
        {
            Debug.LogError("🚨 firePoint가 설정되지 않았습니다! 탄을 발사할 수 없습니다.");
            return;
        }

        // 발사 애니메이션 실행
        animator.SetTrigger("shootTrigger");

        // 탄 생성
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // 일정 시간 후 다시 Idle 상태로 전환
        Invoke(nameof(ResetToIdle), shootCooldown);

        // 쿨타임 적용
        canShoot = false;
        Invoke(nameof(ResetShoot), shootCooldown);
    }

    void ResetShoot()
    {
        canShoot = true;
    }

    void ResetToIdle()
    {
        // 발사 후 다시 Idle 상태로 변경
        animator.Play("PlayerIdle");
    }
}
