using UnityEngine;
using UnityEngine.Video; // VideoPlayer API
using UnityEngine.SceneManagement; // �� ��ȯ

public class OpeningVideo : MonoBehaviour
{
    public VideoPlayer videoPlayer;  // VideoPlayer�� Inspector���� ����
    public string nextSceneName;     // ������ ��� �� �̵��� �� �̸�

    void Start()
    {
        if (videoPlayer == null)
        {
            Debug.LogError("VideoPlayer�� ������� �ʾҽ��ϴ�!");
            return;
        }

        // ������ ��� �Ϸ� ����(���� ����Ʈ) ����
        videoPlayer.loopPointReached += OnVideoEnd;

        // �������� �ڵ� ����ǵ��� VideoPlayer ����
        // (Play On Awake ���� �� Start()���� ���� .Play() ȣ�� ��� ��)
        // videoPlayer.Play();
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        Debug.Log("������ ����� �������ϴ�. ���� ������ �̵��մϴ�.");
        SceneManager.LoadScene(nextSceneName);
    }
}
