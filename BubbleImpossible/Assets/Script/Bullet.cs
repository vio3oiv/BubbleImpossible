using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float delay = 0.5f;
    public Sprite hitSprite; 
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool hasHit = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb.freezeRotation = true;
        rb.linearVelocity = new Vector2(speed, 0f); 
    }
    void Update()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        /*
        if (collision.CompareTag("Enemy"))
        {
            Destroy(gameObject); // ≈∫ ªË¡¶
        }*/

        if (!hasHit && collision.gameObject.tag == "Enemy") 
        {
            hasHit = true;
            spriteRenderer.sprite = hitSprite; 
            rb.linearVelocity = Vector2.zero; 
            Destroy(gameObject, 0.1f); 
        }
    }

}
