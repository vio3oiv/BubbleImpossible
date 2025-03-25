using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject gameOverUI;
    private bool isGameOver = false;

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

        Debug.Log("💀 게임 종료! 3초 후 Game Over UI 표시");

        // 3초 후 UI 표시
        Invoke(nameof(ShowGameOverUI), 1f);
    }

    void ShowGameOverUI()
    {
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
            // GameOver UI 하위에 있는 모든 Animator를 UnscaledTime으로 설정
            Animator[] animators = gameOverUI.GetComponentsInChildren<Animator>();
            foreach (Animator anim in animators)
            {
                anim.updateMode = AnimatorUpdateMode.UnscaledTime;
            }
        }

        Time.timeScale = 0f;
    }


    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
