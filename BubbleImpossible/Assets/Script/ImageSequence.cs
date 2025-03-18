using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ImageSequence : MonoBehaviour
{
    [Header("이미지 시퀀스 설정")]
    public Sprite[] images;           // 차례대로 표시할 이미지들 (배열)
    public float displayTime = 5f;    // 각 이미지가 유지되는 시간
    public float transitionTime = 2f; // 디졸브(전환) 속도 (초)

    [Header("UI 참조")]
    public Image displayImage;        // 이미지를 표시할 UI Image
    public CanvasGroup canvasGroup;   // 디졸브(알파)를 조절하기 위해 사용

    private int currentIndex = 0;

    void Start()
    {
        // CanvasGroup이 없으면 시퀀스 동작이 어려우므로 체크
        if (canvasGroup == null || displayImage == null)
        {
            Debug.LogError("🚨 CanvasGroup 또는 displayImage가 연결되지 않았습니다!");
            return;
        }

        // 처음 이미지는 투명 상태에서 시작
        canvasGroup.alpha = 0f;

        // 첫 번째 이미지 로드(있다면)
        if (images.Length > 0)
        {
            displayImage.sprite = images[0];
        }

        // 시퀀스 시작
        StartCoroutine(PlaySequence());
    }

    IEnumerator PlaySequence()
    {
        // 배열 크기가 0이면 종료
        if (images == null || images.Length == 0)
        {
            Debug.LogWarning("🚨 표시할 이미지가 없습니다!");
            yield break;
        }

        while (currentIndex < images.Length)
        {
            // 1) 현재 이미지를 표시할 준비
            displayImage.sprite = images[currentIndex];

            // 2) 디졸브 인 (알파 0 → 1)
            yield return StartCoroutine(FadeCanvas(0f, 1f, transitionTime));

            // 3) 5초간 유지 (displayTime)
            yield return new WaitForSeconds(displayTime);

            // 4) 디졸브 아웃 (알파 1 → 0)
            yield return StartCoroutine(FadeCanvas(1f, 0f, transitionTime));

            // 다음 이미지로
            currentIndex++;
        }

        // 모든 이미지가 끝난 뒤 씬 전환 또는 다른 로직 수행 가능
        Debug.Log("🎉 모든 이미지 시퀀스 종료!");

        // 예) 씬 전환
        // SceneManager.LoadScene("NextScene");
    }

    // 알파를 서서히 바꿔주는 코루틴 (디졸브 효과)
    IEnumerator FadeCanvas(float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            // 알파 보간
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, t);
            canvasGroup.alpha = newAlpha;

            yield return null;
        }
    }
}
