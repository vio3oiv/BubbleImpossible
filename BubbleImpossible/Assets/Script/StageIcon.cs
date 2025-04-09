using UnityEngine;

/// <summary>
/// 스테이지의 현재 상태를 나타내는 열거형
/// </summary>
public enum StageState
{
    Locked,   // 잠금 상태
    Open,     // 도전 가능 상태
    Cleared   // 클리어 상태
}

/// <summary>
/// 각 스테이지 버튼 프리팹에 부착되어, 상태에 따라 서로 다른 이미지(오브젝트)를 표시합니다.
/// Inspector에서 각 상태별 오브젝트 및 stageIndex를 개별로 할당해야 합니다.
/// </summary>
public class StageIcon : MonoBehaviour
{
    [Header("상태별 오브젝트 (각 스테이지마다 개별 설정)")]
    public GameObject lockedObject;   // 잠금 상태 오브젝트
    public GameObject openObject;     // 개방 상태 오브젝트
    public GameObject clearedObject;  // 클리어 상태 오브젝트

    public StageState currentState = StageState.Locked;
    public int stageIndex; // Inspector에서 스테이지 순서를 지정 (예: 0,1,2,3,4)

    /// <summary>
    /// 지정한 상태로 변경하고 해당 오브젝트만 활성화합니다.
    /// </summary>
    public void SetState(StageState newState)
    {
        Debug.Log($"[Level {stageIndex} Position] >>> SetState({newState}) 호출!", this);
        currentState = newState;

        // 모든 오브젝트 비활성화
        if (lockedObject != null)
        {
            lockedObject.SetActive(false);
            Debug.Log($"[Level {stageIndex} Position] {gameObject.name} - lockedObject 비활성화", this);
        }
        if (openObject != null)
        {
            openObject.SetActive(false);
            Debug.Log($"[Level {stageIndex} Position] {gameObject.name} - openObject 비활성화", this);
        }
        if (clearedObject != null)
        {
            clearedObject.SetActive(false);
            Debug.Log($"[Level {stageIndex} Position] {gameObject.name} - clearedObject 비활성화", this);
        }

        // 새 상태에 따라 해당 오브젝트 활성화
        switch (newState)
        {
            case StageState.Locked:
                if (lockedObject != null)
                {
                    lockedObject.SetActive(true);
                    Debug.Log($"[Level {stageIndex} Position] {gameObject.name} - lockedObject 활성화", this);
                }
                break;
            case StageState.Open:
                if (openObject != null)
                {
                    openObject.SetActive(true);
                    Debug.Log($"[Level {stageIndex} Position] {gameObject.name} - openObject 활성화", this);
                }
                break;
            case StageState.Cleared:
                if (clearedObject != null)
                {
                    clearedObject.SetActive(true);
                    Debug.Log($"[Level {stageIndex} Position] {gameObject.name} - clearedObject 활성화", this);
                }
                else
                {
                    Debug.LogWarning($"[Level {stageIndex} Position] {gameObject.name} - clearedObject가 할당되지 않음", this);
                }
                break;
        }
    }
}
