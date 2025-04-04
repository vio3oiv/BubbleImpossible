using UnityEngine;

public class Trash : MonoBehaviour
{
    // 플레이어에게 줄 피해량
    public int damage = 1;

    // 3D 환경인 경우 OnTriggerEnter(Collider other) 사용 (2D 환경이면 OnTriggerEnter2D로 변경)
    void OnTriggerEnter2D(Collider2D other)
    {
        // 충돌한 객체가 "Player" 태그를 가지고 있는지 확인
        if (other.CompareTag("Player"))
        {
            // Player 스크립트를 가져와서 HP 감소 처리
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
            // 원한다면 충돌 후 쓰레기 오브젝트를 제거합니다.
            Destroy(gameObject);
        }
    }
}
