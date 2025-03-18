using UnityEngine;
using UnityEngine.Video; // VideoPlayer API
using UnityEngine.SceneManagement; // 씬 전환

public class OpeningVideo : MonoBehaviour
{
    public VideoPlayer videoPlayer;  // VideoPlayer를 Inspector에서 연결
    public string nextSceneName;     // 동영상 재생 후 이동할 씬 이름

    void Start()
    {
        if (videoPlayer == null)
        {
            Debug.LogError("VideoPlayer가 연결되지 않았습니다!");
            return;
        }

        // 동영상 재생 완료 시점(루프 포인트) 감지
        videoPlayer.loopPointReached += OnVideoEnd;

        // 동영상이 자동 재생되도록 VideoPlayer 설정
        // (Play On Awake 설정 시 Start()에서 별도 .Play() 호출 없어도 됨)
        // videoPlayer.Play();
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        Debug.Log("동영상 재생이 끝났습니다. 다음 씬으로 이동합니다.");
        SceneManager.LoadScene(nextSceneName);
    }
}
