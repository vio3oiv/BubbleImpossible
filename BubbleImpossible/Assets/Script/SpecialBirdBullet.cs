using UnityEngine;

public class SpecialBirdBullet : MonoBehaviour
{
    public float speed = 7f; // SpecialBird ź �ӵ�
    public int damage = 1; // �÷��̾�� ���� ���ط�
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(-speed, 0f); // ���� �������� �̵��ϵ��� ����
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // �÷��̾ ������ ���
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(damage); // �÷��̾� HP ����
            }
            Destroy(gameObject); // ź ����
        }
        else if (collision.CompareTag("Boundary")) // ���� �浹 �� ����
        {
            Destroy(gameObject);
        }
    }
}
