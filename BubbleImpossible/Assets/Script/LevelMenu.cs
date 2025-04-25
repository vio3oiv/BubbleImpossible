using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelMenu : MonoBehaviour
{
    public GameObject[] buttons;            // 각 스테이지 버튼
    public GameObject[] lockIcons;          // 자물쇠 아이콘
    public GameObject[] lockBlockers;       // 클릭 방지용 오버레이 버튼
    public GameObject lockMessagePanel;     // 잠김 안내 패널

    private void Awake()
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        unlockedLevel = Mathf.Clamp(unlockedLevel, 1, buttons.Length); // 배열 오버 방지

        Debug.Log($"🔓 현재 플레이 가능한 스테이지: {unlockedLevel}");

        for (int i = 0; i < buttons.Length; i++)
        {
            string btnName = buttons[i].name;
            Button btn = buttons[i].GetComponent<Button>();

            bool isUnlocked =  (i + 1 <= unlockedLevel);


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

            Debug.Log($"▶️ [{btnName}] (index: {i}) → {(isUnlocked ? "열림" : "잠김")}");
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
