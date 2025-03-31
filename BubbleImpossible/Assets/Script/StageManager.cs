using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    [Header("미리 배치된 스테이지 버튼 (총 5개)")]
    public List<StageIcon> stageIcons;

    public int currentStageIndex = 0;
    private bool stageClearProcessed = false;

    // Start() 초기화가 한 번만 실행되도록 하는 static 플래그
    private static bool isInitialized = false;

    void Awake()
    {
        // 이 오브젝트를 씬 전환 시에도 유지
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // SaveDataManager 초기화 (저장된 데이터가 없으면 기본값으로 생성)
        SaveDataManager.Initialize(stageIcons.Count);

        // 저장된 세이브 데이터를 기반으로 각 StageIcon의 상태 업데이트
        for (int i = 0; i < stageIcons.Count; i++)
        {
            stageIcons[i].SetState(SaveDataManager.Data.stageStates[i]);
        }
        currentStageIndex = SaveDataManager.Data.currentStageIndex;
    }

    void Update()
    {
        // 테스트용: C 키를 눌러 현재 스테이지 클리어 처리 (한 번만 실행)
        if (Input.GetKeyDown(KeyCode.C) && !stageClearProcessed)
        {
            Debug.Log("임시 StageClear 호출됨!");
            StageClear(currentStageIndex);
            stageClearProcessed = true;
        }
    }

    /// <summary>
    /// 스테이지 클리어 시 호출하는 메서드.
    /// 현재 스테이지는 Cleared로, 다음 스테이지가 있다면 Open으로 전환하고 세이브 데이터를 업데이트합니다.
    /// </summary>
    /// <param name="stageIndex">클리어된 스테이지 인덱스 (0부터 시작)</param>
    public void StageClear(int stageIndex)
    {
        if (stageIndex < 0 || stageIndex >= stageIcons.Count)
        {
            Debug.LogWarning($"잘못된 스테이지 인덱스: {stageIndex}");
            return;
        }

        // 현재 스테이지를 Cleared로 전환
        stageIcons[stageIndex].SetState(StageState.Cleared);
        SaveDataManager.Data.stageStates[stageIndex] = StageState.Cleared;

        // 다음 스테이지가 있다면 Open 상태로 전환
        int nextStageIndex = stageIndex + 1;
        if (nextStageIndex < stageIcons.Count)
        {
            stageIcons[nextStageIndex].SetState(StageState.Open);
            SaveDataManager.Data.stageStates[nextStageIndex] = StageState.Open;
            SaveDataManager.Data.currentStageIndex = nextStageIndex;
        }

        // 변경된 데이터를 저장
        SaveDataManager.Save();
    }

    void OnApplicationQuit()
    {
        // 게임 플레이가 종료되면 저장된 세이브 데이터를 삭제하여 다음 실행 시 초기화되도록 함
        SaveDataManager.ClearSaveData();
    }
}
