using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageClearChecker : MonoBehaviour
{
    [Header("게임 클리어 설정")]
    public GameObject gameClearUI;  // 게임 클리어 UI 패널

    void OnTriggerEnter2D(Collider2D collision)
    {
        // DestroyZone에 적(Enemy)이 닿으면 제거하고 조건 체크
        if (collision.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
            // 적 제거 후 잠시 딜레이를 두고 스테이지 클리어 조건 체크
            Invoke(nameof(CheckStageClear), 0.1f);
        }
    }

    void CheckStageClear()
    {
        // EnemyManager와 Player를 찾습니다.
        EnemyManager enemyManager = FindFirstObjectByType<EnemyManager>();
        Player player = FindFirstObjectByType<Player>();

        if (enemyManager != null && player != null)
        {
            // 조건: 모든 적이 제거되고, 플레이어의 체력이 0이 아닐 때
            if (enemyManager.enemies.Count == 0 && player.hp > 0)
            {
                Debug.Log("🎉 스테이지 클리어 조건 충족! 게임 클리어 UI를 표시합니다.");
                if (gameClearUI != null)
                {
                    gameClearUI.SetActive(true);
                }
            }
        }
    }
}
