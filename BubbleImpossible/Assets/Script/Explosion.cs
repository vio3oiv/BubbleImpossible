using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float destroyDelay = 1f; // 폭발 효과 유지 시간

    void Start()
    {
        Destroy(gameObject, destroyDelay); // 지정된 시간이 지나면 자동 삭제
    }
}
