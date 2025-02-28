using UnityEngine;

public class DestroyZone : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(collision.gameObject); 
    }
}
