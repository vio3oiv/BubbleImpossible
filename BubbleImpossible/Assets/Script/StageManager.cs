using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class StageManager : MonoBehaviour
{
    public static StageManager instance;

    [Header("미리 배치된 스테이지 버튼 (총 5개)")]
    public List<StageIcon> stageIcons;

    public int currentStageIndex = 0;
    private bool stageClearProcessed = false;
    private static bool isInitialized = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }


    void Start()
    {
        if (!isInitialized)
        {
            // 기존 씬에서 초기화
            if (stageIcons == null || stageIcons.Count == 0)
            {
                Debug.LogError("StageManager: stageIcons 리스트가 비어있거나 할당되지 않았습니다.");
                return;
            }

            SaveDataManager.Initialize(stageIcons.Count);

            for (int i = 0; i < stageIcons.Count; i++)
            {
                stageIcons[i].SetState(SaveDataManager.Data.stageStates[i]);
            }
            currentStageIndex = SaveDataManager.Data.currentStageIndex;
            isInitialized = true;
        }
    }

    // 씬이 로드될 때마다 새로운 StageIcon들을 찾아 재할당
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "StageMap")
        {
            // 현재 씬에서 StageIcon 새로 할당
            StageIcon[] newStageIcons = GameObject.FindObjectsOfType<StageIcon>();
            stageIcons = new List<StageIcon>(newStageIcons);
            Debug.Log("새로운 StageIcon들이 할당되었습니다. 개수: " + stageIcons.Count);

            // 만약 새 StageIcon의 개수가 저장된 배열보다 많다면 배열 재설정
            if (stageIcons.Count > SaveDataManager.Data.stageStates.Length)
            {
                StageState[] newStates = new StageState[stageIcons.Count];
                // 기존 배열 복사
                for (int i = 0; i < SaveDataManager.Data.stageStates.Length; i++)
                {
                    newStates[i] = SaveDataManager.Data.stageStates[i];
                }
                // 나머지 기본값 설정
                for (int i = SaveDataManager.Data.stageStates.Length; i < newStates.Length; i++)
                {
                    newStates[i] = StageState.Locked;
                }
                SaveDataManager.Data.stageStates = newStates;
                Debug.Log("SaveDataManager.Data.stageStates 배열이 재설정되었습니다. 새 길이: " + newStates.Length);
            }

            UpdateAllStageIcons();
        }
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && !stageClearProcessed)
        {
            Debug.Log("임시 StageClear 호출됨!");
            StageClear(currentStageIndex);
            stageClearProcessed = true;
        }
    }

    public void StageClear(int stageIndex)
    {
        if (stageIndex < 0 || stageIndex >= stageIcons.Count)
        {
            Debug.LogWarning($"잘못된 스테이지 인덱스: {stageIndex}");
            return;
        }

        stageIcons[stageIndex].SetState(StageState.Cleared);
        SaveDataManager.Data.stageStates[stageIndex] = StageState.Cleared;

        int nextStageIndex = stageIndex + 1;
        if (nextStageIndex < stageIcons.Count)
        {
            stageIcons[nextStageIndex].SetState(StageState.Open);
            SaveDataManager.Data.stageStates[nextStageIndex] = StageState.Open;
            SaveDataManager.Data.currentStageIndex = nextStageIndex;
        }
        SaveDataManager.Save();
    }

    public void UpdateAllStageIcons()
    {
        if (stageIcons == null)
        {
            Debug.LogError("StageManager.UpdateAllStageIcons: stageIcons 리스트가 null입니다.");
            return;
        }

        if (SaveDataManager.Data == null)
        {
            Debug.LogError("StageManager.UpdateAllStageIcons: SaveDataManager.Data가 null입니다.");
            return;
        }

        if (SaveDataManager.Data.stageStates == null)
        {
            Debug.LogError("StageManager.UpdateAllStageIcons: SaveDataManager.Data.stageStates가 null입니다.");
            return;
        }

        for (int i = 0; i < stageIcons.Count; i++)
        {
            if (stageIcons[i] == null)
            {
                Debug.LogWarning($"UpdateAllStageIcons: stageIcons[{i}]가 null입니다.");
                continue;
            }
            if (i >= SaveDataManager.Data.stageStates.Length)
            {
                Debug.LogWarning($"UpdateAllStageIcons: SaveDataManager.Data.stageStates 배열 범위를 초과했습니다. 인덱스: {i}");
                continue;
            }
            stageIcons[i].SetState(SaveDataManager.Data.stageStates[i]);
        }
    }

    void OnApplicationQuit()
    {
        SaveDataManager.ClearSaveData();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
