using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float destroyDelay = 1f; // ���� ȿ�� ���� �ð�

    void Start()
    {
        Destroy(gameObject, destroyDelay); // ������ �ð��� ������ �ڵ� ����
    }
}
