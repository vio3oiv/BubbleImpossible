using UnityEngine;

public class PlayerMovementClamper : MonoBehaviour
{
    public Collider2D moveBounds; // 이동 가능 영역을 정의하는 Collider2D
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogWarning("PlayerMovementClamper: Rigidbody2D 컴포넌트를 찾을 수 없습니다!");
        }
    }

    void FixedUpdate()
    {
        if (moveBounds != null && rb != null)
        {
            Vector2 currentPosition = rb.position;
            Bounds bounds = moveBounds.bounds;
            bool outOfBounds = false;
            Vector2 clampedPosition = currentPosition;

            if (currentPosition.x < bounds.min.x)
            {
                clampedPosition.x = bounds.min.x;
                outOfBounds = true;
            }
            else if (currentPosition.x > bounds.max.x)
            {
                clampedPosition.x = bounds.max.x;
                outOfBounds = true;
            }

            if (currentPosition.y < bounds.min.y)
            {
                clampedPosition.y = bounds.min.y;
                outOfBounds = true;
            }
            else if (currentPosition.y > bounds.max.y)
            {
                clampedPosition.y = bounds.max.y;
                outOfBounds = true;
            }

            if (outOfBounds)
            {
                rb.MovePosition(clampedPosition);
            }
        }
    }
}
