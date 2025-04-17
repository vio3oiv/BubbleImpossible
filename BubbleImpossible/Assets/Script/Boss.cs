using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;       // 기존 UI 관련 네임스페이스 (Slider 등은 그대로 사용)
using TMPro;                // TextMeshPro 네임스페이스 추가

public class Boss : MonoBehaviour
{
    [Header("보스 기본 설정")]
    public int hp = 2;                // 보스 체력
    public bool isBoss = true;        // 보스 여부 (보스 전용 기능 여부)
    public string enemyName;
    public float bulletSpeed = 7f;    // 탄 속도
    public float fireRate = 3f;       // 탄 발사 간격

    // 보스 불렛 프리팹 리스트 (여러 개 중 랜덤 선택)
    public List<GameObject> bossBulletPrefabs;

    public GameObject explosionPrefab;    // 폭발 효과 프리팹

    [Header("보스 이동 설정")]
    public float moveDistance = 3f;   // 위아래 이동 범위
    public float moveSpeed = 2f;      // 이동 속도
    private Vector3 startPosition;
    private bool movingUp = true;

    [Header("보스 발사 설정")]
    // 여러 개의 발사 위치 중 랜덤 선택
    public Transform[] firePoint;

    [Header("보스 타이머 설정")]
    public float bossTimeLimit = 60f;      // 보스 타이머 제한 시간 (초)
    private float bossTimeRemaining;       // 남은 시간
    private Coroutine bossTimerCoroutine;  // 타이머 코루틴
    public TMP_Text bossTimerText;         // 보스 타이머 UI TMP_Text (프리팹에서 생성 후 할당)

    [Header("보스 HP UI 설정")]
    public Slider bossHPSlider;           // 보스 HP를 표시할 슬라이더 UI

    private Animator animator;
    private Transform player;
    public bool isDying = false;

    void Start()
    {
        // 시작 위치 및 컴포넌트 초기화
        startPosition = transform.position;
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        // 보스 HP UI 초기화: 최대값과 현재값을 hp로 설정
        if (bossHPSlider != null)
        {
            bossHPSlider.maxValue = hp;
            bossHPSlider.value = hp;
        }

        // 보스 타이머 초기화: 남은 시간을 bossTimeLimit으로 설정하고 UI 업데이트
        bossTimeRemaining = bossTimeLimit;
        if (bossTimerText != null)
        {
            int minutes = Mathf.FloorToInt(bossTimeRemaining / 60f);
            int seconds = Mathf.FloorToInt(bossTimeRemaining % 60f);
            bossTimerText.text = string.Format("{0}:{1:00}", minutes, seconds);
        }

        // 보스라면 탄 발사 코루틴과 타이머 코루틴 시작
        if (isBoss)
        {
            StartCoroutine(FireRoutine());
            bossTimerCoroutine = StartCoroutine(BossTimer());
        }
    }

    void Update()
    {
        // 보스 위아래 이동 처리
        MoveUpDown();
    }

    /// <summary>
    /// 보스가 fireRate 간격으로 탄을 발사하는 코루틴
    /// </summary>
    IEnumerator FireRoutine()
    {
        
        while (!isDying)
        {
            yield return new WaitForSeconds(fireRate);
            Fire();
        }
    }

    /// <summary>
    /// firePoint 배열 내에서 랜덤한 발사 위치를 선택하고,
    /// bossBulletPrefabs 리스트 내에서 랜덤한 탄 프리팹을 선택하여 탄을 발사합니다.
    /// </summary>
    void Fire()
    {
        SoundManager.instance.PlaySFX(SoundManager.ESfx.SFX_BOSS_SHOOT);
        animator.SetTrigger("OnFire");
        if (bossBulletPrefabs == null || bossBulletPrefabs.Count == 0)
        {
            Debug.LogError("🚨 보스 불렛 프리팹 리스트가 비어 있습니다!");
            return;
        }
        if (firePoint == null || firePoint.Length == 0)
        {
            Debug.LogError("🚨 firePoint 배열이 설정되지 않았습니다!");
            return;
        }

        int randomBulletIndex = Random.Range(0, bossBulletPrefabs.Count);
        GameObject selectedBulletPrefab = bossBulletPrefabs[randomBulletIndex];

        int randomFirePointIndex = Random.Range(0, firePoint.Length);
        Transform chosenFirePoint = firePoint[randomFirePointIndex];

        GameObject bullet = Instantiate(selectedBulletPrefab, chosenFirePoint.position, Quaternion.identity);
        if (bullet == null)
        {
            Debug.LogError("🚨 보스 탄 생성 실패!");
            return;
        }

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("🚨 보스 탄에 Rigidbody2D가 없습니다!");
            return;
        }

        // 탄을 왼쪽으로 발사 (필요 시 방향 수정)
        rb.linearVelocity = Vector2.left * bulletSpeed;
        Debug.Log($"🚀 보스가 탄을 발사했습니다! 속도: {rb.linearVelocity}");
    }

    /// <summary>
    /// 보스가 위아래로 이동합니다.
    /// </summary>
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

    /// <summary>
    /// 보스 타이머 코루틴: bossTimeLimit부터 카운트다운하며, UI를 매 프레임 업데이트합니다.
    /// 시간이 다 되면, 예를 들어 플레이어의 패배 처리(GameManager.instance.GameOver())를 호출합니다.
    /// </summary>
    IEnumerator BossTimer()
    {
        while (bossTimeRemaining > 0)
        {
            bossTimeRemaining -= Time.deltaTime;
            if (bossTimerText != null)
            {
                int minutes = Mathf.FloorToInt(bossTimeRemaining / 60f);
                int seconds = Mathf.FloorToInt(bossTimeRemaining % 60f);
                bossTimerText.text = string.Format("{0}:{1:00}", minutes, seconds);
            }
            yield return null;
        }

        // 시간이 다 되었을 때 (보스 타이머 종료)
        if (!isDying)
        {
            SoundManager.instance.PlaySFX(SoundManager.ESfx.SFX_TIMER_END);
            Debug.Log("보스 타이머 종료: 시간이 다 되었습니다!");
            if (GameManager.instance != null)
            {
                GameManager.instance.GameOver();
            }
        }
    }


    /// <summary>
    /// 충돌 처리:
    /// - 플레이어 탄(예: 태그 "BossBullet")과 충돌 시 보스 체력 감소 및 사망 처리
    /// - 플레이어와 충돌 시 플레이어에게 데미지 전달
    /// </summary>
    void OnTriggerEnter2D(Collider2D collision)
    {
        // 보스 탄과 충돌 처리
        if (collision.CompareTag("BossBullet"))
        {
            // BossSpecialBullet 컴포넌트가 있다면 변환 여부 확인
            BossSpecialBullet specialBullet = collision.GetComponent<BossSpecialBullet>();
            if (specialBullet != null && !specialBullet.IsTransformed)
            {
                // 변환되기 전이면 보스 HP에 영향을 주지 않음
                return;
            }

            // 변환되었거나 BossSpecialBullet 스크립트가 없으면 보스 HP 차감
            SoundManager.instance.PlaySFX(SoundManager.ESfx.SFX_BOSS_HIT);
            hp -= 1;
            Debug.Log($"🚨 보스 체력: {hp}");
            animator.SetTrigger("OnAttack");
            // 보스 HP 슬라이더 UI 업데이트
            if (bossHPSlider != null)
            {
                bossHPSlider.value = hp;
            }
            if (hp <= 0 && !isDying)
            {
                isDying = true;
                animator.SetTrigger("OnDeath");
                // 타이머 코루틴 중지
                if (bossTimerCoroutine != null)
                {
                    StopCoroutine(bossTimerCoroutine);
                    bossTimerCoroutine = null;
                }
                StartCoroutine(FlyUpAndDestroy());
            }
            Destroy(collision.gameObject);
        }
        // 플레이어와의 충돌 처리
        else if (collision.CompareTag("Player"))
        {
            Player playerScript = collision.GetComponent<Player>();
            if (playerScript != null)
            {
                if (!isDying)
                {
                    playerScript.TakeDamage(1);
                }
                else
                {
                    playerScript.ForceIdle();
                }
            }
        }
    }


    /// <summary>
    /// 보스가 사망 애니메이션 후 위로 날아오르며 파괴됩니다.
    /// </summary>
    IEnumerator FlyUpAndDestroy()
    {
        float flySpeed = 2f;
        float duration = 1f;
        float timer = 0f;

        // 보스가 위로 날아오르는 연출
        while (timer < duration)
        {
            transform.position += Vector3.up * flySpeed * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }

        // 폭발 이펙트 생성 (설정되어 있을 경우)
        if (explosionPrefab != null)
        {
            SoundManager.instance.PlaySFX(SoundManager.ESfx.SFX_EXPLOSION);
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }

        
        if (GameManager.instance != null)
        {
            GameManager.instance.BossGameClear();
        }

        Destroy(gameObject);
    }



}
