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

            // 저장된 데이터에 stageStates 배열이 부족하면 확장
            if (Data.stageStates == null || Data.stageStates.Length < stageCount)
            {
                StageState[] newStates = new StageState[stageCount];
                int oldLength = (Data.stageStates != null) ? Data.stageStates.Length : 0;
                for (int i = 0; i < oldLength; i++)
                    newStates[i] = Data.stageStates[i];
                for (int i = oldLength; i < stageCount; i++)
                    newStates[i] = StageState.Locked;
                if (oldLength == 0)
                    newStates[0] = StageState.Open;
                Data.stageStates = newStates;
            }
        }
        else
        {
            Data = new SaveDataContainer();
            Data.stageStates = new StageState[stageCount];
            Data.stageStates[0] = StageState.Open;
            for (int i = 1; i < stageCount; i++)
                Data.stageStates[i] = StageState.Locked;
            Data.currentStageIndex = 0;
            Save();
        }
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
}
