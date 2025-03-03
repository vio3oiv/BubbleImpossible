using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    public List<Enemy> enemies = new List<Enemy>(); // ��� ���� ����Ʈ�� ����
    public GameObject explosionPrefab; // ���� ȿ�� ������

    void Update()
    {
        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            if (enemies[i] != null && enemies[i].hp <= 0)
            {
                StartCoroutine(DestroyEnemyWithDelay(enemies[i]));
            }
        }
    }

    public void RegisterEnemy(Enemy enemy)
    {
        enemies.Add(enemy);
    }

    public IEnumerator DestroyEnemyWithDelay(Enemy enemy)
    {
        if (enemy == null) yield break; // ���� �̹� �����Ǿ����� �ߴ�

        Animator animator = enemy.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("OnDeath"); // ��� �ִϸ��̼� ����
        }

        yield return new WaitForSeconds(1f); // 1�� ���

        if (enemy == null) yield break; // ���� �����Ǿ����� �ߴ�

        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, enemy.transform.position, Quaternion.identity);
        }

        enemies.Remove(enemy);
        Destroy(enemy.gameObject);
    }
}
