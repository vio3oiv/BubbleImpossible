using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [Header("Ʃ�丮�� UI")]
    public GameObject tutorialPanel; // Ʃ�丮�� �ȳ� �г�
    public Button startButton;       // "����" ��ư (Ʃ�丮�� �ݱ�)

    void Start()
    {
        // ���� ���� �� �Ͻ�����
        Time.timeScale = 0f;

        // Ʃ�丮�� �г� ǥ��
        tutorialPanel.SetActive(true);

        // ��ư Ŭ�� �� TutorialEnd �Լ� ����
        startButton.onClick.AddListener(TutorialEnd);
    }

    void TutorialEnd()
    {
        // Ʃ�丮�� �г� ��Ȱ��ȭ
        tutorialPanel.SetActive(false);

        // ���� �簳
        Time.timeScale = 1f;
    }
}
