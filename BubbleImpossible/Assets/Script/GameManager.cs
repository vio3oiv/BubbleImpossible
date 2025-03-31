using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("UI Panels")]
    public GameObject gameOverUI;  // 게임 오버 UI 패널
    public GameObject gameClearUI; // 게임 클리어 UI 패널

    [Header("Stage Manager 연동")]
    public StageManager stageManager; // StageManager에 배치된 스테이지 버튼 관리 스크립트
    public int totalStages = 5;       // 전체 스테이지 개수

    private int currentStageIndex = 0; // 현재 클리어된 스테이지 인덱스 (0부터 시작)
    private bool isGameOver = false;
    private bool isGameClear = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))  // C 키 눌러서 클리어 테스트
        {
            Debug.Log("임시 StageClear 호출됨!");
            StageCompleted();  //스테이지를 클리어했다고 가정
        }
    }

    /// <summary>
    /// 외부(예: 스테이지 클리어 조건을 확인하는 로직)에서 호출하여, 현재 스테이지가 클리어되었음을 알림.
    /// 해당 스테이지 버튼은 클리어 상태로 변경되고, 다음 스테이지 버튼은 개방 상태로 전환됩니다.
    /// 모든 스테이지가 클리어되면 전체 게임 클리어 처리(GameClear())가 호출됩니다.
    /// </summary>
    public void StageCompleted()
    {
        // StageManager가 연결되어 있다면, 현재 스테이지 버튼 상태를 업데이트
        if (stageManager != null)
        {
            stageManager.StageClear(currentStageIndex);
        }
        else
        {
            Debug.LogWarning("GameManager: StageManager가 할당되지 않았습니다!");
        }

        currentStageIndex++;

        // 모든 스테이지 클리어 후에도 추가 동작 없이 각 스테이지 상태가 유지됨
        if (currentStageIndex >= totalStages)
        {
            GameClear();
            Debug.Log("모든 스테이지를 클리어했습니다.");
            // 이곳에 추가적인 메시지나 효과를 넣을 수 있습니다.
        }
    }

    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        Debug.Log("💀 게임 종료! 1초 후 Game Over UI 표시");
        Invoke(nameof(ShowGameOverUI), 1f);
    }

    void ShowGameOverUI()
    {
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }
        // 필요시: Time.timeScale = 0f;
    }

    public void GameClear()
    {
        if (isGameClear) return;
        isGameClear = true;

        Debug.Log("🎉 게임 클리어!");
        // 1초 후에 전체 게임 클리어 UI 표시
        Invoke(nameof(ShowGameClearUI), 1f);
    }

    private void ShowGameClearUI()
    {
        if (gameClearUI != null)
        {
            gameClearUI.SetActive(true);

            Animator[] animators = gameClearUI.GetComponentsInChildren<Animator>();
            foreach (Animator anim in animators)
            {
                // 시간 정지 상태에서도 애니메이션이 동작하도록 설정
                anim.updateMode = AnimatorUpdateMode.UnscaledTime;
            }
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


}