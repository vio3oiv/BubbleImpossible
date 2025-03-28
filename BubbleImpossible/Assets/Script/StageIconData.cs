using UnityEngine;

[System.Serializable]
public class StageIconData
{
    [Header("스테이지 상태별 오브젝트")]
    public GameObject lockedObject;   // 잠금 상태 아이콘 프리팹 (또는 오브젝트)
    public GameObject openObject;     // 개방 상태 아이콘 프리팹 (또는 오브젝트)
    public GameObject clearedObject;  // 클리어 상태 아이콘 프리팹 (또는 오브젝트)
}
