using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    public List<Enemy> enemies = new List<Enemy>();

    void Start()
    {
        
        enemies.Add(new Enemy("Bird", 2, 3, 3f, "-", "-"));
        enemies.Add(new Enemy("FatBird", 4, 5, 5f, "-", "-"));
        enemies.Add(new Enemy("HomingBird", 2, 2, 4f, "-", "-"));
        enemies.Add(new Enemy("SpecialBird", 3, 3, 2f, "�� ������ ���� ������ ���¿��� �߻�ü �߻�", "speed: 1/s time: 3"));
        enemies.Add(new Enemy("SpeedBird", 2, 2, 2f, "-", "-"));
        enemies.Add(new Enemy("FatBirdEX", 5, 5, 5f, "-", "-"));

        
        PrintEnemyList();
    }

   
    void PrintEnemyList()
    {
        foreach (Enemy enemy in enemies)
        {
            Debug.Log($"�̸�: {enemy.name}, HP: {enemy.hp}, �ӵ�: {enemy.speed}, Ư������: {enemy.specialAttack}");
        }
    }
}
