using UnityEngine;
using System.Collections;

public class AppleSpawner : MonoBehaviour
{
    public GameObject[] applePrefabs;    // 스폰할 사과 프리팹 배열
    public float minSpawnInterval = 2f;    // 최소 스폰 간격 (초)
    public float maxSpawnInterval = 5f;    // 최대 스폰 간격 (초)

    // 스폰 위치에 추가할 오프셋 (2D 게임에서도 사용 가능)
    public Vector3 spawnOffset = Vector3.zero;

    private void Start()
    {
        if (applePrefabs == null || applePrefabs.Length == 0)
        {
            Debug.LogError("🚨 사과 프리팹 배열이 비어 있습니다!");
            return;
        }

        StartCoroutine(SpawnAppleCoroutine());
        Debug.Log("사과 스폰 시작됨!");
    }

    private IEnumerator SpawnAppleCoroutine()
    {
        while (true)
        {
            SpawnApple();
            float interval = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(interval);
        }
    }

    void SpawnApple()
    {
        // 스폰 위치 계산 (오프셋 포함)
        Vector3 spawnPosition = transform.position + spawnOffset;

        // 무작위 사과 프리팹 선택
        GameObject randomApplePrefab = applePrefabs[Random.Range(0, applePrefabs.Length)];
        GameObject apple = Instantiate(randomApplePrefab, spawnPosition, Quaternion.identity);

    }
}
