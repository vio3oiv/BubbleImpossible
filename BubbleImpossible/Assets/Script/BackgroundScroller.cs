using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScrolling : MonoBehaviour
{
    #region Inspector

    public Renderer scrollRenderer;
    public float speed = 1f;

    #endregion

    private void Update()
    {
        float move = Time.deltaTime * speed;
        scrollRenderer.material.mainTextureOffset += Vector2.right * move;
    }

    void Awake()
    {
        if (scrollRenderer == null)
        {
            scrollRenderer = GetComponent<Renderer>();
            if (scrollRenderer == null)
            {
                Debug.LogError("🚨 Renderer를 찾을 수 없습니다! " +
                               "BackgroundScroller에 Renderer 컴포넌트가 필요합니다.");
            }
        }
    }
}