using UnityEngine;

[System.Serializable]
public class Enemy
{
    public string name;      // 적 이름
    public int hp;          // 체력
    public int hp0;         // HP가 0이 되었을 때 표시
    public float speed;     // 속도
    public string specialAttack; // 특수 공격 설명
    public string notes;    // 비고

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
