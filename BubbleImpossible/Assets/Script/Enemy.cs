using UnityEngine;

[System.Serializable]
public class Enemy
{
    public string name;      // �� �̸�
    public int hp;          // ü��
    public int hp0;         // HP�� 0�� �Ǿ��� �� ǥ��
    public float speed;     // �ӵ�
    public string specialAttack; // Ư�� ���� ����
    public string notes;    // ���

    public Enemy(string name, int hp, int hp0, float speed, string specialAttack, string notes)
    {
        this.name = name;
        this.hp = hp;
        this.hp0 = hp0;
        this.speed = speed;
        this.specialAttack = specialAttack;
        this.notes = notes;
    }
}
