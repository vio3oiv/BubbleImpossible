using UnityEngine;
using System.Collections.Generic;

public class StageManager : MonoBehaviour
{
    [Header("미리 배치된 스테이지 버튼 (총 5개)")]
    // 씬에서 미리 배치된 StageIcon 컴포넌트를 가진 오브젝트들을 Inspector에서 순서대로 할당합니다.
    public List<StageIcon> stageIcons;

    void Start()
    {
        // 초기 상태 설정: 첫 번째 스테이지는 Open, 나머지는 Locked 상태로 설정
        for (int i = 0; i < stageIcons.Count; i++)
        {
            if (i == 0)
                stageIcons[i].SetState(StageState.Open);
            else
                stageIcons[i].SetState(StageState.Locked);
        }
    }

    /// <summary>
    /// 외부(예: 게임매니저)에서 스테이지 클리어 시 호출하는 메서드
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

        // 현재 스테이지 버튼을 Cleared 상태로 전환
        stageIcons[stageIndex].SetState(StageState.Cleared);

        // 다음 스테이지 버튼이 있다면 Open 상태로 전환
        int nextStageIndex = stageIndex + 1;
        if (nextStageIndex < stageIcons.Count)
        {
            stageIcons[nextStageIndex].SetState(StageState.Open);
        }
    }
}
