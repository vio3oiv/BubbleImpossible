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
/// Inspector에서 각 상태별 오브젝트를 개별로 할당할 수 있습니다.
/// </summary>
public class StageIcon : MonoBehaviour
{
    [Header("상태별 오브젝트 (각 스테이지마다 개별 설정)")]
    public GameObject lockedObject;   // 잠금 상태에서 보여질 오브젝트 
    public GameObject openObject;     // 개방 상태에서 보여질 오브젝트 
    public GameObject clearedObject;  // 클리어 상태에서 보여질 오브젝트 

    public StageState currentState = StageState.Locked;

    /// <summary>
    /// 스테이지 상태를 변경하고, 해당 상태에 맞는 오브젝트만 활성화합니다.
    /// </summary>
    public void SetState(StageState newState)
    {
        currentState = newState;

        // 모든 상태 오브젝트를 비활성화
        if (lockedObject != null) lockedObject.SetActive(false);
        if (openObject != null) openObject.SetActive(false);
        if (clearedObject != null) clearedObject.SetActive(false);

        // 새 상태에 따라 해당 오브젝트 활성화
        switch (newState)
        {
            case StageState.Locked:
                if (lockedObject != null)
                    lockedObject.SetActive(true);
                break;
            case StageState.Open:
                if (openObject != null)
                    openObject.SetActive(true);
                break;
            case StageState.Cleared:
                if (clearedObject != null)
                    clearedObject.SetActive(true);
                break;
        }
    }
}
