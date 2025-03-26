using UnityEngine;
using System.Collections.Generic;

public class PatternManager : MonoBehaviour
{
    public List<GameObject> patterns; // 패턴 프리팹 리스트
    private int currentPatternIndex = -1;
    private GameObject currentPattern;

    void Start()
    {
        if (patterns.Count > 0)
        {
            NextPattern();
        }
        else
        {
            Debug.LogError("🚨 패턴 리스트가 비어 있습니다!");
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
            Destroy(currentPattern);
        }

        currentPatternIndex = index;
        Vector3 patternPosition = patterns[currentPatternIndex].transform.position;
        currentPattern = Instantiate(patterns[currentPatternIndex], patternPosition, Quaternion.identity);
        SpawnEnemiesFromPattern(currentPattern);
        Debug.Log($"🚀 {currentPatternIndex + 1} 번째 패턴 시작! 위치: {patternPosition}");
    }

    public void NextPattern()
    {
        LoadPattern(currentPatternIndex + 1);
    }

    void SpawnEnemiesFromPattern(GameObject pattern)
    {
        Enemy[] enemies = pattern.GetComponentsInChildren<Enemy>();
        EnemyManager enemyManager = FindFirstObjectByType<EnemyManager>();

        foreach (Enemy enemy in enemies)
        {
            enemy.gameObject.SetActive(true);
            enemyManager?.RegisterEnemy(enemy);
        }
    }

    // 마지막 패턴 여부 확인 (예시)
    public bool IsLastPattern()
    {
        return currentPatternIndex >= patterns.Count - 1;
    }
}
