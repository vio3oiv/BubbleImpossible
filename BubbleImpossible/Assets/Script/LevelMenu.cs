using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelMenu : MonoBehaviour
{
    public GameObject[] buttons;            // 버튼 GameObject 배열
    public GameObject[] lockIcons;          // 자물쇠 아이콘 GameObject 배열
    public GameObject[] lockBlockers;       // 잠김 상태 클릭 방지 및 메시지 출력용 오버레이
    public GameObject lockMessagePanel;

    private void Awake()
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        Debug.Log($"🔓 저장된 언락 스테이지: {unlockedLevel}");

        for (int i = 0; i < buttons.Length; i++)
        {
            string btnName = buttons[i].name;
            Button btn = buttons[i].GetComponent<Button>();
            bool isUnlocked = i < unlockedLevel;

            btn.interactable = isUnlocked;

            if (lockIcons[i] != null)
            {
                lockIcons[i].SetActive(!isUnlocked);
                Debug.Log($"🔒 [{btnName}] LockIcon {(isUnlocked ? "비활성화됨" : "활성화됨")}");
            }

            if (lockBlockers[i] != null)
            {
                lockBlockers[i].SetActive(!isUnlocked);
                Debug.Log($"🛑 [{btnName}] LockBlocker {(isUnlocked ? "비활성화됨" : "활성화됨")}");

                if (!isUnlocked)
                {
                    Button blockerBtn = lockBlockers[i].GetComponent<Button>();
                    blockerBtn.onClick.RemoveAllListeners();
                    blockerBtn.onClick.AddListener(() =>
                    {
                        Debug.Log($"❗ [{btnName}] 은/는 잠겨 있습니다.");
                        ShowLockedMessage();
                    });
                }
            }

            Debug.Log($"▶️ [{btnName}] (index: {i}) → {(isUnlocked ? "언락됨" : "잠겨있음")}");
        }
    }

    void ShowLockedMessage()
    {
        Debug.Log("📢 잠김 메시지 패널 활성화됨");
        if (lockMessagePanel != null)
            lockMessagePanel.SetActive(true);
        else
            Debug.LogWarning("⚠️ lockMessagePanel이 설정되지 않았습니다!");
    }
}
