﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("UI Panels")]
    public GameObject gameOverUI;  // 게임 오버 UI 패널
    public GameObject gameClearUI; // 스테이지 클리어 UI 패널
    public GameObject tutorialPanel; //

    [Header("Boss Game Clear UI")]
    public GameObject bossGameClearUI; // 보스 게임 클리어 UI 패널

    [Header("Stage Settings")]
    public int totalStages = 5;       // 전체 스테이지 개수

    private int currentStageIndex = 0; // 현재 클리어된 스테이지 인덱스 (0부터 시작)
    private bool isGameOver = false;
    private bool isGameClear = false;
    // 외부에서 GameClear()가 호출될 때, 최초 1회에 한해 StageCompleted()를 실행할지 여부 플래그
    private bool stageCompletedOnGameClear = false;

    // 보스 게임 클리어를 위한 상태 변수 (보스 객체가 하나이므로 별도로 관리)
    private bool isBossGameClear = false;

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

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))  // C 키 눌러서 클리어 테스트 (스테이지 클리어)
        {
            Debug.Log("임시 StageClear 호출됨!");
            StageCompleted();  // 스테이지를 클리어했다고 가정
        }
        if (Time.timeScale == 0f)
        {
            // (2) 튜토리얼 중이라면 절대 재개하지 않음
            if (tutorialPanel != null && tutorialPanel.activeSelf)
                return;

            // (3) 나머지 모든 UI가 꺼져 있으면 재개
            bool overOff = gameOverUI == null || !gameOverUI.activeSelf;
            bool clearOff = gameClearUI == null || !gameClearUI.activeSelf;
            bool bossOff = bossGameClearUI == null || !bossGameClearUI.activeSelf;

            if (overOff && clearOff && bossOff)
            {
                Time.timeScale = 1f;
                isGameOver = false;
                isGameClear = false;
                isBossGameClear = false;
                stageCompletedOnGameClear = false;
            }
        }
    }



        /// <summary>
        /// 스테이지 클리어 처리 메서드
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
            Time.timeScale = 1f;
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

    public void StageCompleted()
    {
        currentStageIndex++;

        // 🔓 다음 스테이지 언락 저장
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        if (unlockedLevel < currentStageIndex + 1)
        {
            PlayerPrefs.SetInt("UnlockedLevel", currentStageIndex + 1);
            PlayerPrefs.Save();
            Debug.Log($"🔓 언락된 스테이지: {currentStageIndex + 1}");
        }

        if (currentStageIndex >= totalStages)
        {
            Debug.Log("모든 스테이지를 클리어했습니다.");
            GameClear();
        }
    }



    /// <summary>
    /// 보스가 죽었을 때 호출됩니다.
    /// 보스 객체는 하나이므로, 보스가 사망하면 항상 보스 게임 클리어 UI를 띄웁니다.
    /// </summary>
    public void BossGameClear()
        {
            if (isBossGameClear) return;

            isBossGameClear = true;
            Debug.Log("🎉 보스가 사망하여 보스 게임 클리어!");
            // 1초 후 보스 게임 클리어 UI 표시
            Invoke(nameof(OnBossGameClear), 1f);
        }

        void OnBossGameClear()
        {
            Time.timeScale = 0f;
            if (bossGameClearUI != null)
            {
                bossGameClearUI.SetActive(true);

                Animator[] animators = bossGameClearUI.GetComponentsInChildren<Animator>();
                foreach (Animator anim in animators)
                {
                    // 시간 정지 상태에서도 애니메이션이 동작하도록 설정
                    anim.updateMode = AnimatorUpdateMode.UnscaledTime;
                }
            }
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
            Time.timeScale = 0f;
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

