/*using UnityEngine;

[System.Serializable]
public class SaveDataContainer
{
    public StageState[] stageStates; // 각 스테이지의 상태 배열
    public int currentStageIndex;     // 현재 진행 중인 스테이지 인덱스
}

public static class SaveDataManager
{
    private const string SaveKey = "GameSaveData";
    public static SaveDataContainer Data { get; private set; }

    /// <summary>
    /// 게임 시작 시(새 게임) 무조건 기본 상태로 초기화합니다.
    /// 이전에 저장된 데이터가 있더라도 무시하고 데이터를 새로 생성합니다.
    /// 게임 실행 중에는 Data가 유지되어 진행 상황을 관리합니다.
    /// </summary>
    public static void Initialize(int stageCount)
    {
        // 이전 데이터와 상관없이, 새 게임 시작 시 무조건 기본 데이터로 초기화합니다.
        Data = new SaveDataContainer();
        Data.stageStates = new StageState[stageCount];
        Data.stageStates[0] = StageState.Open;
        for (int i = 1; i < stageCount; i++)
        {
            Data.stageStates[i] = StageState.Locked;
        }
        Data.currentStageIndex = 0;
        Save();
    }

    public static void Save()
    {
        string json = JsonUtility.ToJson(Data);
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();
    }

    public static void ClearSaveData()
    {
        PlayerPrefs.DeleteKey(SaveKey);
        Data = null;
    }
}*/
