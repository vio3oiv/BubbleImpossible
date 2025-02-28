using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 5f; // �̵� �ӵ�
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // �Է� ���� (�¿� + ���� �̵�)
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        // �̵� ���� ���� ����
        movement = new Vector2(moveX, moveY).normalized; // �밢�� �̵� �ӵ� ����

        // �÷��̾� ���� ����
        transform.localScale = new Vector3(1, 1, 1);
        spriteRenderer.flipX = false;
    }

    void FixedUpdate()
    {
        // Rigidbody2D�� �̿��� ���� �̵�
        rb.linearVelocity = movement * speed;
    }
}
