using UnityEngine;
using System.Collections.Generic;

public class StageManager : MonoBehaviour
{
    [Header("미리 배치된 스테이지 버튼 (총 5개)")]
    // 씬에서 미리 배치된 StageIcon 컴포넌트를 가진 오브젝트들을 Inspector에서 순서대로 할당합니다.
    public List<StageIcon> stageIcons;

    private bool stageClearProcessed = false;
    public int currentStageIndex = 0;

    private static bool isInitialized = false;

    void Awake()
    {
        // 이 오브젝트를 씬 전환 시에도 유지
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // 아직 초기화되지 않은 경우에만 초기화 실행
        if (!isInitialized)
        {
            // 최초 초기화: 첫 번째 스테이지는 Open, 나머지는 Locked
            for (int i = 0; i < stageIcons.Count; i++)
            {
                if (i == 0)
                    stageIcons[i].SetState(StageState.Open);
                else
                    stageIcons[i].SetState(StageState.Locked);
            }
            isInitialized = true;
        }
    }

    void Update()
    {
        // C 키를 눌러서 임시로 StageClear()를 테스트합니다.
        if (Input.GetKeyDown(KeyCode.C) && !stageClearProcessed)
        {
            Debug.Log("임시 StageClear 호출됨!");
            StageClear(currentStageIndex);
            stageClearProcessed = true;
        }
    }

    /// <summary>
    /// 외부(예: 게임매니저)에서 스테이지 클리어 시 호출하는 메서드.
    /// 현재 스테이지 버튼은 Cleared로 변경되고, 다음 스테이지가 있다면 Open 상태로 전환됩니다.
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

        // 다음 스테이지가 있다면 Open 상태로 전환
        int nextStageIndex = stageIndex + 1;
        if (nextStageIndex < stageIcons.Count)
        {
            stageIcons[nextStageIndex].SetState(StageState.Open);
        }
    }
}
