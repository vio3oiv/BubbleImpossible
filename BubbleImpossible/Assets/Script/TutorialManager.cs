using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [Header("TutorialUI")]
    public GameObject tutorialPanel; 
    public Button startButton;       

    void Start()
    {
        
        Time.timeScale = 0f;

        
        tutorialPanel.SetActive(true);

        
        startButton.onClick.AddListener(TutorialEnd);
    }

    void TutorialEnd()
    {
        
        tutorialPanel.SetActive(false);

        
        Time.timeScale = 1f;
    }
}
