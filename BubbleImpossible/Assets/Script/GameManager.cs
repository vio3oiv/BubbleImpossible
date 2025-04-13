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
    
    //public StageManager stageManager; // StageManager에 배치된 스테이지 버튼 관리 스크립트

    public int totalStages = 5;       // 전체 스테이지 개수

    private int currentStageIndex = 0; // 현재 클리어된 스테이지 인덱스 (0부터 시작)
    private bool isGameOver = false;
    private bool isGameClear = false;
    // 외부에서 GameClear()가 호출될 때, 최초 1회에 한해 StageCompleted()를 실행할지 여부 플래그
    private bool stageCompletedOnGameClear = false;

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
            StageCompleted();  // 스테이지를 클리어했다고 가정
        }
    }

    /// <summary>
    /// 외부(예: 스테이지 클리어 조건 확인 로직)에서 호출하여 해당 스테이지가 클리어되었음을 알림.
    /// StageManager의 상태 업데이트 및 다음 스테이지 버튼 개방 등이 처리됩니다.
    /// 모든 스테이지 클리어 시 GameClear()를 호출합니다.
    /// </summary>
    public void StageCompleted()
    {
        // StageManager 관련 코드 주석 처리
        /*
        if (stageManager != null)
        {
            stageManager.StageClear(currentStageIndex);
        }
        else
        {
            Debug.LogWarning("GameManager: StageManager가 할당되지 않았습니다!");
        }
        */

        currentStageIndex++;

        // 모든 스테이지가 클리어되면 게임 클리어를 호출
        if (currentStageIndex >= totalStages)
        {
            Debug.Log("모든 스테이지를 클리어했습니다.");
            GameClear();
        }
    }

    /// <summary>
    /// 게임 클리어 처리 메서드
    /// 외부(예: 씬에서)로부터 GameClear()가 호출될 경우, 
    /// 현재 스테이지 진행 상태가 완료되지 않았다면 최초 1회에 한해 StageCompleted()를 실행하여 스테이지 UI 상태를 업데이트합니다.
    /// </summary>
    public void GameClear()
    {
        // 만약 외부에서 GameClear()가 호출되었는데 아직 모든 스테이지가 클리어되지 않았다면...
        // (예: 디버깅이나 다른 조건에 의해 GameClear()가 직접 호출된 경우)
        if (!stageCompletedOnGameClear && currentStageIndex < totalStages)
        {
            stageCompletedOnGameClear = true;
            // 강제로 현재 스테이지를 마지막 스테이지 직전으로 설정한 후 StageCompleted() 실행
            // StageCompleted()가 currentStageIndex를 증가시켜 총 스테이지 개수에 도달하도록 함
            currentStageIndex = totalStages - 1;
            StageCompleted();
            return;  // StageCompleted()에서 GameClear()가 재호출되도록 함
        }

        // 게임 클리어가 이미 처리되었다면 재실행 방지
        if (isGameClear)
            return;

        isGameClear = true;
        Debug.Log("🎉 게임 클리어!");
        // 1초 후 게임 클리어 UI 표시
        Invoke(nameof(OnGameClear), 1f);
    }

    void OnGameClear()
    {
        if (gameClearUI != null)
        {
            gameClearUI.SetActive(true);
        }
        // StageManager 관련 코드 주석 처리
        /*
        if (stageManager != null)
        {
            stageManager.UpdateAllStageIcons();
        }
        */
    }

    private void ShowGameOverUI()
    {
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);

            Animator[] animators = gameOverUI.GetComponentsInChildren<Animator>();
            foreach (Animator anim in animators)
            {
                // 시간 정지 상태에서도 애니메이션이 동작하도록 설정
                anim.updateMode = AnimatorUpdateMode.UnscaledTime;
            }
        }
    }

    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        Debug.Log("💀 게임 종료! 1초 후 Game Over UI 표시");
        Invoke(nameof(ShowGameOverUI), 1f);
    }

    public void RestartGame()
    {
        Debug.Log("💀 게임 재시작");
        //SaveDataManager.ClearSaveData(); // 저장된 데이터 초기화
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
