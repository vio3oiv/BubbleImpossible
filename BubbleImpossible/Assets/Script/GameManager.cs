using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject gameOverUI;  // 게임 오버 UI 패널
    public GameObject gameClearUI; // 게임 클리어 UI 패널
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
        //Time.timeScale = 0f;
    }

    // 새로 추가: 게임 클리어 처리
    public void GameClear()
    {
        if (isGameClear) return;
        isGameClear = true;

        Debug.Log("🎉 게임 클리어!");
        if (gameClearUI != null)
        {
            gameClearUI.SetActive(true);

            Animator[] animators = gameClearUI.GetComponentsInChildren<Animator>();
            foreach (Animator anim in animators)
            {
                anim.updateMode = AnimatorUpdateMode.UnscaledTime;
            }
        }

        Debug.Log("💀 게임 종료! 1초 후 Game Creal UI 표시");
        Invoke(nameof(gameClearUI), 1f);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}