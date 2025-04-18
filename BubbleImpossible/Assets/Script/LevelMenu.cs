using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelMenu : MonoBehaviour
{
    public Button[] buttons;              // 레벨 버튼 배열
    public GameObject levelButtons;       // 버튼들을 자식으로 가진 오브젝트

    private void Awake()
    {
        ButtonsToArray(); // 자식 오브젝트에서 버튼 배열 구성

        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1); // 기본값 1 (스테이지 1만 언락됨)

        // 모든 버튼 비활성화
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }

        // 언락된 스테이지까지만 버튼 활성화
        for (int i = 0; i < unlockedLevel && i < buttons.Length; i++)
        {
            buttons[i].interactable = true;
        }
    }

    // 레벨 로드 함수 (levelId는 빌드 세팅에 등록된 인덱스 또는 씬 이름)
    public void OpenLevel(int levelId)
    {
        SceneManager.LoadScene(levelId);
    }

    // levelButtons의 자식들을 Button 배열로 변환
    void ButtonsToArray()
    {
        int childCount = levelButtons.transform.childCount;
        buttons = new Button[childCount];

        for (int i = 0; i < childCount; i++)
        {
            buttons[i] = levelButtons.transform.GetChild(i).GetComponent<Button>();
        }
    }
}
