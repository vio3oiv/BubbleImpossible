using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    public List<Enemy> enemies = new List<Enemy>(); // 적 리스트
    public GameObject explosionPrefab; // 폭발 효과 프리팹

    void Update()
    {
        // hp <= 0인 적들을 제거하는 코루틴 실행
        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            if (enemies[i] != null && enemies[i].hp <= 0)
            {
                StartCoroutine(DestroyEnemyWithDelay(enemies[i]));
            }
        }

        // 리스트에서 null 항목 제거
        enemies.RemoveAll(e => e == null);

        // 적 리스트가 비었으면 GameManager로 게임 클리어 처리를 위임
        if (enemies.Count == 0)
        {
            // 패턴이 남아 있다면 다음 패턴 실행
            PatternManager patternManager = FindFirstObjectByType<PatternManager>();
            if (patternManager != null && !patternManager.IsLastPattern())
            {
                patternManager.NextPattern();
            }
            else
            {
                // 패턴이 모두 끝났다면 GameManager에서 게임 클리어 처리
                GameManager.instance.GameClear();
            }
        }
    }

    public void RegisterEnemy(Enemy enemy)
    {
        enemies.Add(enemy);
    }

    public IEnumerator DestroyEnemyWithDelay(Enemy enemy)
    {
        if (enemy == null) yield break;

        Animator animator = enemy.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("OnDeath");
        }

        yield return new WaitForSeconds(1f);

        if (enemy == null) yield break;

        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, enemy.transform.position, Quaternion.identity);
        }

        enemies.Remove(enemy);
        Destroy(enemy.gameObject);
    }
}
