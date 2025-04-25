using UnityEngine;

public class AutoPlayerPrefsReset : MonoBehaviour
{
    void Awake()
    {
        Debug.Log("🧹 게임 실행 - 저장 초기화");

        PlayerPrefs.DeleteAll();              // 모든 저장 데이터 초기화
        PlayerPrefs.SetInt("UnlockedLevel", 1); // 1스테이지만 오픈
        PlayerPrefs.Save();

        Debug.Log("✅ UnlockedLevel = 1 저장 완료");
    }
}
