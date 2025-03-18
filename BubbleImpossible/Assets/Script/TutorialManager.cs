using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [Header("튜토리얼 UI")]
    public GameObject tutorialPanel; // 튜토리얼 안내 패널
    public Button startButton;       // "시작" 버튼 (튜토리얼 닫기)

    void Start()
    {
        // 게임 시작 전 일시정지
        Time.timeScale = 0f;

        // 튜토리얼 패널 표시
        tutorialPanel.SetActive(true);

        // 버튼 클릭 시 TutorialEnd 함수 실행
        startButton.onClick.AddListener(TutorialEnd);
    }

    void TutorialEnd()
    {
        // 튜토리얼 패널 비활성화
        tutorialPanel.SetActive(false);

        // 게임 재개
        Time.timeScale = 1f;
    }
}
