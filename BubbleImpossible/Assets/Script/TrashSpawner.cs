using UnityEngine;
using System.Collections;

public class TrashSpawner : MonoBehaviour
{
    public GameObject[] trashPrefabs;    // 스폰할 쓰레기 프리팹 배열
    public float minSpawnInterval = 2f;    // 최소 스폰 간격 (초)
    public float maxSpawnInterval = 5f;    // 최대 스폰 간격 (초)

    // (선택사항) 스폰 위치에 추가할 오프셋
    public Vector3 spawnOffset = Vector3.zero;

    // 떨어지는 속도를 제어하기 위한 변수
    public float fallSpeed = 5f;           // 쓰레기가 떨어지는 속도 (단위: m/s)

    private void Start()
    {
        if (trashPrefabs == null || trashPrefabs.Length == 0)
        {
            Debug.LogError("🚨 쓰레기 프리팹 배열이 비어 있습니다!");
            return;
        }

        StartCoroutine(SpawnTrashCoroutine());
        Debug.Log("쓰레기 스폰 시작됨!");
    }

    private IEnumerator SpawnTrashCoroutine()
    {
        while (true)
        {
            SpawnTrash();
            float interval = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(interval);
        }
    }

    void SpawnTrash()
    {
        // 스폰 위치 계산 (오프셋 포함)
        Vector3 spawnPosition = transform.position + spawnOffset;

        // 무작위 쓰레기 프리팹 선택
        GameObject randomTrashPrefab = trashPrefabs[Random.Range(0, trashPrefabs.Length)];
        GameObject trash = Instantiate(randomTrashPrefab, spawnPosition, Quaternion.identity);

        // Rigidbody 설정 (3D 게임일 경우 Rigidbody, 2D 게임이면 Rigidbody2D 사용)
        Rigidbody rb = trash.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // 중력 사용하지 않고 일정 속도로 떨어지도록 설정
            rb.useGravity = false;
            rb.linearVelocity = new Vector3(0, -fallSpeed, 0);
        }
    }
}
