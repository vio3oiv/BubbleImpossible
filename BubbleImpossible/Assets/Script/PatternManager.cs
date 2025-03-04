using UnityEngine;
using System.Collections.Generic;

public class PatternManager : MonoBehaviour
{
    public List<GameObject> patterns; // Unity 에디터에서 추가할 패턴 프리팹 리스트
    private int currentPatternIndex = -1;
    private GameObject currentPattern;

    void Start()
    {
        if (patterns.Count > 0)
        {
            NextPattern(); // 첫 번째 패턴 자동 실행
        }
        else
        {
            Debug.LogError("🚨 패턴 리스트가 비어 있습니다! Unity 에디터에서 패턴을 추가하세요.");
        }
    }

    void LoadPattern(int index)
    {
        if (index >= patterns.Count)
        {
            Debug.Log("🎉 모든 패턴이 완료되었습니다!");
            return;
        }

        if (currentPattern != null)
        {
            Destroy(currentPattern); // 기존 패턴 제거
        }

        currentPatternIndex = index;

        // 패턴이 설정된 원래 위치에서 생성되도록 변경
        Vector3 patternPosition = patterns[currentPatternIndex].transform.position;
        currentPattern = Instantiate(patterns[currentPatternIndex], patternPosition, Quaternion.identity);

        // 패턴 내의 적들을 원래 위치에서 활성화
        SpawnEnemiesFromPattern(currentPattern);

        Debug.Log($"🚀 {currentPatternIndex + 1} 번째 패턴 시작! 위치: {patternPosition}");
    }

    public void NextPattern()
    {
        LoadPattern(currentPatternIndex + 1);
    }

    void SpawnEnemiesFromPattern(GameObject pattern)
    {
        Enemy[] enemies = pattern.GetComponentsInChildren<Enemy>(); // 패턴 내 모든 적 찾기
        EnemyManager enemyManager = FindFirstObjectByType<EnemyManager>();

        foreach (Enemy enemy in enemies)
        {
            enemy.gameObject.SetActive(true); // 원래 위치에서 활성화
            enemyManager?.RegisterEnemy(enemy); // 적을 EnemyManager에 등록
        }
    }
}
