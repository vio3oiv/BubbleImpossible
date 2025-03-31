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

    /// <summary>
    /// 저장된 세이브 데이터가 있으면 불러오고, 없으면 기본값으로 초기화합니다.
    /// </summary>
    /// <param name="stageCount">전체 스테이지 개수</param>
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
    /// 게임 플레이가 종료될 때 호출되어 세이브 데이터를 삭제합니다.
    /// </summary>
    public static void ClearSaveData()
    {
        PlayerPrefs.DeleteKey(SaveKey);
        Data = null;
    }
}
