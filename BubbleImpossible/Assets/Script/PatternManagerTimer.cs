using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PatternManagerTimer : MonoBehaviour
{
    public List<GameObject> patterns; // 패턴 프리팹 리스트
    public float delayBetweenPatterns = 5f; // 패턴 사이의 딜레이 (초)

    private int currentPatternIndex = 0;
    private GameObject currentPattern;
    private Coroutine patternCoroutine;

    void Start()
    {
        if (patterns.Count > 0)
        {
            patternCoroutine = StartCoroutine(PatternRoutine());
        }
        else
        {
            Debug.LogError("🚨 패턴 리스트가 비어 있습니다!");
        }
    }

    IEnumerator PatternRoutine()
    {
        while (currentPatternIndex < patterns.Count)
        {
            LoadPattern(currentPatternIndex);
            currentPatternIndex++;
            yield return new WaitForSeconds(delayBetweenPatterns);
        }
        Debug.Log("🎉 모든 패턴이 완료되었습니다!");
    }

    /// <summary>
    /// 지정한 인덱스의 패턴을 로드하여 인스턴스화합니다.
    /// </summary>
    /// <param name="index">패턴 인덱스</param>
    void LoadPattern(int index)
    {
        if (index >= patterns.Count)
        {
            Debug.Log("🚨 유효하지 않은 패턴 인덱스입니다.");
            return;
        }

        if (currentPattern != null)
        {
            Destroy(currentPattern);
        }

        Vector3 patternPosition = patterns[index].transform.position;
        currentPattern = Instantiate(patterns[index], patternPosition, Quaternion.identity);

        SpawnEnemiesFromPattern(currentPattern);
        Debug.Log($"🚀 {index + 1} 번째 패턴 시작! 위치: {patternPosition}");
    }

    /// <summary>
    /// 로드된 패턴의 하위 오브젝트 중 Enemy 컴포넌트를 활성화하고,
    /// EnemyManager에 등록합니다.
    /// </summary>
    /// <param name="pattern">인스턴스화된 패턴 GameObject</param>
    void SpawnEnemiesFromPattern(GameObject pattern)
    {
        Enemy[] enemies = pattern.GetComponentsInChildren<Enemy>();

        EnemyManager enemyManager = Object.FindFirstObjectByType<EnemyManager>();

        foreach (Enemy enemy in enemies)
        {
            enemy.gameObject.SetActive(true);
            enemyManager?.RegisterEnemy(enemy);
        }
    }
}
