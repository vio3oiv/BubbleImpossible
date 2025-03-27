using UnityEngine;
using System.Collections.Generic;

public class StageManager : MonoBehaviour
{
    [Header("Stage UI Containers")]
    // 각 스테이지 버튼이 표시될 빈 컨테이너 오브젝트들을 순서대로 할당 (예: Stage1, Stage2, ...)
    public List<GameObject> stageButtonContainers;

    [Header("Stage Prefabs")]
    public GameObject openPrefab;           // 스테이지1에 사용할 개방 상태 프리팹
    public GameObject lockedPrefab;         // 아직 열리지 않은 스테이지에 공통으로 사용할 잠금 상태 프리팹
    // 각 스테이지에 대해 클리어된 상태의 프리팹 (인덱스 0~N-1에 대응)
    public List<GameObject> clearedPrefabs;

    // 현재 진행 중인(열린) 스테이지 인덱스 (0부터 시작)
    private int currentStageIndex = 0;

    void Start()
    {
        // 초기 UI 설정: 첫 번째 스테이지는 개방 상태, 나머지는 잠금 상태로 표시
        for (int i = 0; i < stageButtonContainers.Count; i++)
        {
            GameObject prefabToInstantiate = (i == 0) ? openPrefab : lockedPrefab;
            InstantiatePrefabInContainer(stageButtonContainers[i], prefabToInstantiate);
        }
    }

    /// <summary>
    /// 스테이지 클리어 시 호출.
    /// 해당 스테이지 컨테이너의 프리팹을 클리어 상태 프리팹으로 교체합니다.
    /// </summary>
    /// <param name="stageIndex">0부터 시작하는 스테이지 인덱스</param>
    public void StageClear(int stageIndex)
    {
        if (stageIndex < 0 || stageIndex >= stageButtonContainers.Count)
        {
            Debug.LogWarning("잘못된 스테이지 인덱스: " + stageIndex);
            return;
        }

        // 기존 프리팹 제거
        foreach (Transform child in stageButtonContainers[stageIndex].transform)
        {
            Destroy(child.gameObject);
        }

        // 해당 스테이지에 대한 클리어 프리팹을 인스턴스화
        if (stageIndex < clearedPrefabs.Count && clearedPrefabs[stageIndex] != null)
        {
            InstantiatePrefabInContainer(stageButtonContainers[stageIndex], clearedPrefabs[stageIndex]);
        }
        else
        {
            Debug.LogWarning("스테이지 " + (stageIndex + 1) + "의 클리어 프리팹이 설정되지 않았습니다.");
        }
    }

    // 지정된 컨테이너에 프리팹 인스턴스 생성 (애니메이션이 자연스럽게 재생되도록)
    private void InstantiatePrefabInContainer(GameObject container, GameObject prefab)
    {
        if (container == null || prefab == null)
            return;

        GameObject instance = Instantiate(prefab, container.transform);
        instance.transform.localPosition = Vector3.zero;
        instance.transform.localScale = Vector3.one;
    }
}
