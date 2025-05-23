﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI; // UI Image를 쓰려면 필요

public class Player : MonoBehaviour
{
    [Header("플레이어 기본 스탯")]
    public int hp = 3;
    private int maxHP;                 // 최대 HP (Start()에서 초기 hp로 설정)
    public float speed = 5f;
    private bool isDead = false;
    private bool canShoot = true;
    private bool isInvulnerable = false;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 movement;

    [Header("공격 관련")]
    public Transform firePoint;
    public GameObject bulletPrefab;
    public Collider2D moveBounds;
    public float shootCooldown = 0.2f;

    [Header("풍선(HP) 관련")]
    public GameObject[] balloonObjects;
    public GameObject balloonPopEffect;

    [Header("특수 스킬")]
    public int maxSpecialSkillCount = 3;   // 최대 사용 가능 횟수
    private int currentSkillCount;         // 현재 남은 횟수
    public Image[] skillUIImages;          // 특수 스킬 UI (이미지 배열)
    public float skillInvulTime = 1f;      // 스킬 사용 시 무적 시간
    public Sprite usedSkillSprite; // 스킬 사용 후 변경될 이미지(아이콘)
    public GameObject specialSkillEffectPrefab; // 스킬 사용 시 나타날 이펙트 프리팹

    [Header("플레이어 HP UI 설정")]
    public Image[] hpUIImages;         // 체력을 표시할 UI 이미지 배열
    public Sprite fullHPSprite;        // 채워진 HP (예: 활성 하트)
    public Sprite emptyHPSprite;       // 비어있는 HP (예: 비활성 하트)

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // 시작 시 Idle 애니메이션
        animator.Play("PlayerIdle");

        // 특수 스킬 횟수 초기화
        currentSkillCount = maxSpecialSkillCount;
        UpdateSkillUI();

        maxHP = hp;
        UpdateHPUI();
    }

    void Update()
    {
        if (isDead) return;

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        Vector2 movement = new Vector2(moveX, moveY).normalized;

        float isMoving = (movement != Vector2.zero) ? 1f : 0f;
        animator.SetFloat("isMoving", isMoving);

        rb.linearVelocity = movement * speed;

        if (Input.GetKeyDown(KeyCode.Z) && canShoot)
        {
            Shoot();
        }

        // 특수 스킬 사용 (키: X)
        if (Input.GetKeyDown(KeyCode.X))
        {
            UseSpecialSkill();
        }
    }

    void FixedUpdate()
    {
        if (!isDead && moveBounds != null && rb != null)
        {
            Vector2 currentPos = rb.position;
            Bounds bounds = moveBounds.bounds;

            // 현재 위치가 영역 밖에 있는지 확인
            bool isOutOfBounds = (currentPos.x < bounds.min.x || currentPos.x > bounds.max.x ||
                                  currentPos.y < bounds.min.y || currentPos.y > bounds.max.y);

            if (isOutOfBounds)
            {
                Vector2 clampedPosition = new Vector2(
                    Mathf.Clamp(currentPos.x, bounds.min.x, bounds.max.x),
                    Mathf.Clamp(currentPos.y, bounds.min.y, bounds.max.y)
                );

                rb.MovePosition(clampedPosition);
            }
        }
    }

    void Shoot()
    {
        if (firePoint == null)
        {
            Debug.LogError("🚨 firePoint가 설정되지 않았습니다!");
            return;
        }

        animator.SetTrigger("shootTrigger");
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        // 공격 사운드 재생
        SoundManager.instance.PlaySFX(SoundManager.ESfx.SFX_SHOOT);

        Invoke(nameof(ResetToIdle), shootCooldown);
        canShoot = false;
        Invoke(nameof(ResetShoot), shootCooldown);
    }

    void ResetShoot()
    {
        canShoot = true;
    }

    void ResetToIdle()
    {
        animator.Play("PlayerIdle");
    }

    // ================================
    // (1) 데미지 로직
    // ================================
    public void TakeDamage(int damage)
    {
        if (isInvulnerable) return;

        hp -= damage;
        Debug.Log($"🚨 플레이어 HP: {hp}");

        PopBalloon(); // 풍선 제거 로직
        // HP UI 업데이트 (체력 UI 이미지 갱신)
        UpdateHPUI();

        StartCoroutine(InvulnerabilityRoutine(1f)); // 1초 무적

        SoundManager.instance.PlaySFX(SoundManager.ESfx.SFX_DAMAGE);


        if (hp > 0)
        {
            animator.SetTrigger("HitTrigger");
        }

        if (hp <= 0 && !isDead)
        {
            isDead = true;
            animator.SetTrigger("DeathTrigger");
            StartCoroutine(Die());
        }
    }

    void PopBalloon()
    {
        if (hp < 0) return;
        if (hp >= balloonObjects.Length) return;

        GameObject balloon = balloonObjects[hp];
        if (balloon != null)
        {
            if (balloonPopEffect != null)
            {
                Instantiate(balloonPopEffect, balloon.transform.position, Quaternion.identity);
            }
            Destroy(balloon);
            balloonObjects[hp] = null;
        }
    }

    IEnumerator InvulnerabilityRoutine(float duration)
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(duration);
        isInvulnerable = false;
    }

    // ================================
    // (2) 특수 스킬 로직
    // ================================
    void UseSpecialSkill()
    {
        if (currentSkillCount <= 0)
        {
            Debug.LogWarning("🚨 특수 스킬을 더 이상 사용할 수 없습니다!");
            return;
        }

        currentSkillCount--;
        Debug.Log($"🎉 Special Skill Used! 남은 횟수: {currentSkillCount}");

        UpdateSkillUI(); // UI 업데이트

        // 스킬 사용 시 이펙트 효과 재생 (플레이어 위치에서 효과를 나타냄)
        if (specialSkillEffectPrefab != null)
        {
            Instantiate(specialSkillEffectPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("🚨 특수 스킬 이펙트 프리팹이 설정되지 않았습니다!");
        }

        // 특수 스킬 사운드 재생
        SoundManager.instance.PlaySFX(SoundManager.ESfx.SFX_SPECIALSKILL);

        // 1초 무적
        StartCoroutine(InvulnerabilityRoutine(skillInvulTime));

        // 전 맵의 적 처치
        KillAllEnemies();
    }


    void KillAllEnemies()
    {
        // 씬 내 모든 Enemy를 찾습니다.
        Enemy[] allEnemies = Object.FindObjectsByType<Enemy>(FindObjectsSortMode.None);

        foreach (Enemy enemy in allEnemies)
        {
            if (enemy != null && !enemy.isDying)
            {
                enemy.hp = 0; // HP를 0으로 만들어 적을 죽임
                Debug.Log($"💀 {enemy.name} was killed by Special Skill!");

                // EnemyManager가 있다면 사망 처리를 지연시켜 실행
                EnemyManager mgr = Object.FindFirstObjectByType<EnemyManager>();
                if (mgr != null)
                {
                    StartCoroutine(mgr.DestroyEnemyWithDelay(enemy));
                }
                else
                {
                    // EnemyManager가 없으면 바로 Destroy 호출
                    Destroy(enemy.gameObject);
                }
            }
        }
    }


    void UpdateSkillUI()
    {
        for (int i = 0; i < skillUIImages.Length; i++)
        {
            // 모든 UI Image를 활성화(비활성화 안 함)
            skillUIImages[i].enabled = true;

            if (i < currentSkillCount)
            {
                // 스킬 남아있는 아이콘 (기존 이미지를 유지하든, 다른 스프라이트든)
                // 예: skillUIImages[i].sprite = availableSkillSprite;
                // 만약 이미 “사용 가능” 아이콘이면, 따로 안 바꿔도 됨.
            }
            else
            {
                // i >= currentSkillCount → 이미 사용된 스킬
                skillUIImages[i].sprite = usedSkillSprite;
            }
        }
    }

     // ================================
    // (3) HP UI 업데이트 로직
    // ================================
    void UpdateHPUI()
    {
        if (hpUIImages == null || hpUIImages.Length == 0)
            return;

        // 예시: hpUIImages 배열의 길이는 최대 HP와 같다고 가정
        for (int i = 0; i < hpUIImages.Length; i++)
        {
            // i가 현재 hp 값보다 작으면 체력 있음 (full), 아니면 체력이 없는 상태 (empty)
            if (i < hp)
            {
                hpUIImages[i].sprite = fullHPSprite;
            }
            else
            {
                hpUIImages[i].sprite = emptyHPSprite;
            }
        }
    }

    // ================================
    // (3) 사망 로직
    // ================================
    IEnumerator Die()
    {
        Debug.Log("💀 플레이어 사망! 즉시 사망 애니메이션");
        yield return new WaitForSeconds(0.1f);
        // 사망 사운드 재생
        SoundManager.instance.PlaySFX(SoundManager.ESfx.SFX_DAMAGE);
        rb.gravityScale = 1f;
        rb.linearVelocity = new Vector2(0, -5f);
        GameManager.instance.GameOver();
    }


    // ================================
    // (4) 충돌 처리
    // ================================
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            TakeDamage(1);
        }
    }

    public void ForceIdle()
    {
        // 플레이어 애니메이터를 Idle로 전환
        animator.Play("PlayerIdle");
    }

    public void ChangeAnimationOnHit()
    {
        animator.SetTrigger("specialAttack");
    }

}
