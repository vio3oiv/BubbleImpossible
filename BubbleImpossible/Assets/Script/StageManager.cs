/*using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class StageManager : MonoBehaviour
{
    public static StageManager instance;

    // StageIcon 리스트: 씬 로드 시 새롭게 검색하여 할당합니다.
    public List<StageIcon> stageIcons;

    // 중복 UI 업데이트를 방지하기 위한 플래그
    private bool uiUpdatedOnSceneLoad = false;

    void Awake()
    {
        // 싱글톤 패턴: 첫 인스턴스만 유지하고 나머지는 파괴합니다.
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        /*
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        // OnSceneLoaded()에서 이미 UI 업데이트가 완료되었다면 Start에서는 추가 업데이트를 하지 않습니다.
        if (uiUpdatedOnSceneLoad)
            return;

        if (stageIcons == null || stageIcons.Count == 0)
        {
            Debug.LogError("StageManager: stageIcons 리스트가 할당되지 않았습니다. (Inspector 설정을 확인하세요.)");
            return;
        }

        // GameManager에서 SaveDataManager를 초기화한 후 진행되는 상황을 가정합니다.
        if (SaveDataManager.Data == null || SaveDataManager.Data.stageStates == null)
            SaveDataManager.Initialize(stageIcons.Count);

        UpdateAllStageIcons();
    }

    // 씬 전환 시 호출되는 이벤트 핸들러
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "StageMap")
        {
            // 모든 StageIcon을 검색 (비활성 객체 포함) 후 태그 "StageButton" 기준으로 필터링
            StageIcon[] allFoundIcons = Object.FindObjectsByType<StageIcon>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            List<StageIcon> filteredIcons = new List<StageIcon>();
            foreach (StageIcon icon in allFoundIcons)
            {
                if (icon == null)
                    continue;
                if (icon.CompareTag("StageButton"))
                    filteredIcons.Add(icon);
            }
            if (filteredIcons.Count == 0)
            {
                Debug.LogWarning("StageMap 씬에서 태그 'StageButton'을 가진 StageIcon을 찾을 수 없습니다.");
                return;
            }

            // StageIcon들을 정렬 후 할당
            filteredIcons.Sort((a, b) => a.stageIndex.CompareTo(b.stageIndex));
            stageIcons = filteredIcons;
            Debug.Log($"정렬 후 할당된 StageIcon 개수: {stageIcons.Count}");
            foreach (StageIcon icon in stageIcons)
            {
                Debug.Log($"[Sorted] StageIcon: {icon.gameObject.name}, stageIndex: {icon.stageIndex}");
            }

            // SaveDataManager 초기화 여부 확인 및 강제 초기화
            if (SaveDataManager.Data == null || SaveDataManager.Data.stageStates == null)
            {
                if (stageIcons.Count > 0)
                {
                    Debug.LogWarning("SaveDataManager가 초기화되어 있지 않습니다. 초기화 진행합니다.");
                    SaveDataManager.Initialize(stageIcons.Count);
                }
                else
                {
                    Debug.LogError("StageManager: stageIcons 리스트가 비어있어 SaveDataManager를 초기화할 수 없습니다.");
                    return;
                }
            }
            
            // 혹시 stageIcons 개수보다 배열 길이가 부족하면 배열 확장
            else if (stageIcons.Count > SaveDataManager.Data.stageStates.Length)
            {
                int oldLength = SaveDataManager.Data.stageStates.Length;
                StageState[] newStates = new StageState[stageIcons.Count];
                for (int i = 0; i < oldLength; i++)
                    newStates[i] = SaveDataManager.Data.stageStates[i];
                for (int i = oldLength; i < newStates.Length; i++)
                    newStates[i] = StageState.Locked;
                SaveDataManager.Data.stageStates = newStates;
                Debug.Log($"SaveDataManager.Data.stageStates 배열 재설정됨. 새 길이: {newStates.Length}");
            }

            UpdateAllStageIcons();
            uiUpdatedOnSceneLoad = true;
        }
    }


    /// <summary>
    /// SaveDataManager의 stageStates 배열에 따라 각 StageIcon의 상태를 업데이트합니다.
    /// </summary>
    public void UpdateAllStageIcons()
    {
        if (stageIcons == null)
        {
            Debug.LogError("StageManager.UpdateAllStageIcons: stageIcons 리스트가 null입니다.");
            return;
        }
        if (SaveDataManager.Data == null || SaveDataManager.Data.stageStates == null)
        {
            Debug.LogError("StageManager.UpdateAllStageIcons: SaveDataManager 데이터가 null입니다.");
            return;
        }
        int count = Mathf.Min(stageIcons.Count, SaveDataManager.Data.stageStates.Length);
        for (int i = 0; i < count; i++)
        {
            stageIcons[i].SetState(SaveDataManager.Data.stageStates[i]);
        }
    }

    /// <summary>
    /// 지정된 스테이지 인덱스에 해당하는 스테이지를 클리어 처리한 후,
    /// 다음 스테이지가 Locked 상태라면 Open으로 변경합니다.
    /// 변경 사항은 저장 후 UI에 반영됩니다.
    /// </summary>
    /// <param name="stageIndex">클리어된 스테이지 인덱스 (0부터 시작)</param>
    public void StageClear(int stageIndex)
    {
        if (SaveDataManager.Data == null || SaveDataManager.Data.stageStates == null)
        {
            Debug.LogError("SaveDataManager 데이터가 초기화되지 않았습니다.");
            return;
        }
        if (stageIndex < 0 || stageIndex >= SaveDataManager.Data.stageStates.Length)
        {
            Debug.LogWarning($"유효하지 않은 스테이지 인덱스: {stageIndex}");
            return;
        }
        Debug.Log($"[Before StageClear] Data.stageStates[{stageIndex}] = {SaveDataManager.Data.stageStates[stageIndex]}");

        // 현재 스테이지를 Cleared로 변경
        SaveDataManager.Data.stageStates[stageIndex] = StageState.Cleared;

        // 다음 스테이지가 있고 상태가 Locked라면 Open으로 변경
        if (stageIndex + 1 < SaveDataManager.Data.stageStates.Length &&
            SaveDataManager.Data.stageStates[stageIndex + 1] == StageState.Locked)
        {
            SaveDataManager.Data.stageStates[stageIndex + 1] = StageState.Open;
        }
        SaveDataManager.Save();
        Debug.Log($"[After StageClear] Data.stageStates[{stageIndex}] = {SaveDataManager.Data.stageStates[stageIndex]}");
        UpdateAllStageIcons();
    }

    void OnApplicationQuit()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}*/
