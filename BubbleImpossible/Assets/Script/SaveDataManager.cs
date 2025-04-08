using UnityEngine;

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

  
    public static void Initialize(int stageCount)
    {
        if (PlayerPrefs.HasKey(SaveKey))
        {
            string json = PlayerPrefs.GetString(SaveKey);
            Data = JsonUtility.FromJson<SaveDataContainer>(json);
        }
        else
        {
            Data = new SaveDataContainer();
            Data.stageStates = new StageState[stageCount];

            // 기본값: 첫 번째 스테이지는 Open, 나머지는 Locked 상태
            Data.stageStates[0] = StageState.Open;
            for (int i = 1; i < stageCount; i++)
            {
                Data.stageStates[i] = StageState.Locked;
            }
            Data.currentStageIndex = 0;
            Save();
        }
    }

    /// <summary>
    /// 현재 데이터를 PlayerPrefs에 저장합니다.
    /// </summary>
    public static void Save()
    {
        string json = JsonUtility.ToJson(Data);
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// 게임 종료 시 세이브 데이터를 삭제하여 다음 실행 시 초기화되도록 합니다.
    /// </summary>
    public static void ClearSaveData()
    {
        PlayerPrefs.DeleteKey(SaveKey);
        Data = null;
    }
}
