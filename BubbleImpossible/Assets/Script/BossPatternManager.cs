using UnityEngine;
using System.Collections.Generic;

public class BossPatternManager : MonoBehaviour
{
    [Header("보스 패턴 프리팹 리스트")]
    public List<GameObject> bossPatterns;  // 보스 패턴 프리팹들을 순서대로 할당합니다.

    private int currentBossPatternIndex = -1;
    private GameObject currentBossPattern;

    void Start()
    {
        // 보스 패턴을 바로 시작할 것이라면 주석을 제거합니다.
        // 단, 기존 패턴 매니저가 끝난 후 BossPatternManager를 활성화하도록 할 수도 있습니다.
        if (bossPatterns.Count > 0)
        {
            NextBossPattern();
        }
        else
        {
            Debug.LogError("🚨 보스 패턴 리스트가 비어 있습니다!");
        }
    }

    /// <summary>
    /// 지정된 인덱스의 보스 패턴 프리팹을 로드하여 인스턴스화하고, 해당 패턴의 보스(또는 적)를 활성화합니다.
    /// </summary>
    /// <param name="index">보스 패턴 인덱스 (0부터 시작)</param>
    void LoadBossPattern(int index)
    {
        if (index >= bossPatterns.Count)
        {
            Debug.Log("🎉 모든 보스 패턴이 완료되었습니다!");
            // 모든 보스 패턴이 완료되었을 때 추가 행동(예: 보스 사망, 보스 종료 처리)을 구현할 수 있습니다.
            return;
        }

        if (currentBossPattern != null)
        {
            Destroy(currentBossPattern);
        }

        currentBossPatternIndex = index;
        Vector3 patternPosition = bossPatterns[currentBossPatternIndex].transform.position;
        currentBossPattern = Instantiate(bossPatterns[currentBossPatternIndex], patternPosition, Quaternion.identity);

        // 보스 패턴 내의 보스(또는 적)들을 활성화 및 등록합니다.
        SpawnBossEnemiesFromPattern(currentBossPattern);

        Debug.Log($"🚀 보스 패턴 {currentBossPatternIndex + 1} 시작! 위치: {patternPosition}");
    }

    /// <summary>
    /// 다음 보스 패턴을 로드합니다.
    /// </summary>
    public void NextBossPattern()
    {
        LoadBossPattern(currentBossPatternIndex + 1);
    }

    /// <summary>
    /// 보스 패턴 프리팹 내의 Boss 컴포넌트를 가진 오브젝트들을 활성화하고, EnemyManager에 등록합니다.
    /// 만약 일반 Enemy 스크립트를 사용한다면, 아래 코드를 수정하세요.
    /// </summary>
    /// <param name="pattern">인스턴스화된 보스 패턴 프리팹</param>
    void SpawnBossEnemiesFromPattern(GameObject pattern)
    {
        // Boss 스크립트를 사용하는 경우
        Boss[] bossEnemies = pattern.GetComponentsInChildren<Boss>();
        EnemyManager enemyManager = FindObjectOfType<EnemyManager>();

        foreach (Boss bossEnemy in bossEnemies)
        {
            if (bossEnemy != null)
            {
                bossEnemy.gameObject.SetActive(true);
                enemyManager?.RegisterEnemy(bossEnemy);
            }
        }
    }

    /// <summary>
    /// 현재 인스턴스된 보스 패턴이 마지막 패턴인지 확인합니다.
    /// </summary>
    /// <returns>마지막 패턴이면 true, 아니면 false</returns>
    public bool IsLastBossPattern()
    {
        return currentBossPatternIndex >= bossPatterns.Count - 1;
    }
}
