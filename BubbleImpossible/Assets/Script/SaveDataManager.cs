using UnityEngine;

[System.Serializable]
public class SaveDataContainer
{
    public StageState[] stageStates; // �� ���������� ���� �迭
    public int currentStageIndex;     // ���� ���� ���� �������� �ε���
}

public static class SaveDataManager
{
    private const string SaveKey = "GameSaveData";

    public static SaveDataContainer Data { get; private set; }

    /// <summary>
    /// ����� ���̺� �����Ͱ� ������ �ҷ�����, ������ �⺻������ �ʱ�ȭ�մϴ�.
    /// </summary>
    /// <param name="stageCount">��ü �������� ����</param>
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

            // �⺻��: ù ��° ���������� Open, �������� Locked ����
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
    /// ���� �����͸� PlayerPrefs�� �����մϴ�.
    /// </summary>
    public static void Save()
    {
        string json = JsonUtility.ToJson(Data);
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// ���� �÷��̰� ����� �� ȣ��Ǿ� ���̺� �����͸� �����մϴ�.
    /// </summary>
    public static void ClearSaveData()
    {
        PlayerPrefs.DeleteKey(SaveKey);
        Data = null;
    }
}
