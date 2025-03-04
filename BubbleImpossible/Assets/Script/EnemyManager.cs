using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    public List<Enemy> enemies = new List<Enemy>(); // 적 리스트
    public GameObject explosionPrefab; // 폭발 효과 프리팹

    void Update()
    {
        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            if (enemies[i] != null && enemies[i].hp <= 0)
            {
                StartCoroutine(DestroyEnemyWithDelay(enemies[i]));
            }
        }

        enemies.RemoveAll(e => e == null);

        if (enemies.Count == 0)
        {
            Debug.Log("✅ 모든 적이 제거됨, 다음 패턴 실행");
            FindFirstObjectByType<PatternManager>()?.NextPattern();
        }
    }

    public void RegisterEnemy(Enemy enemy)
    {
        enemies.Add(enemy);
    }

    public IEnumerator DestroyEnemyWithDelay(Enemy enemy)
    {
        if (enemy == null) yield break; // 적이 이미 삭제되었으면 중단

        Animator animator = enemy.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("OnDeath"); // 사망 애니메이션 실행
        }

        yield return new WaitForSeconds(1f); // 1초 대기 (사망 애니메이션 유지)

        if (enemy == null) yield break; // 적이 삭제되었으면 중단

        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, enemy.transform.position, Quaternion.identity);
        }

        enemies.Remove(enemy);
        Destroy(enemy.gameObject);
    }
}
