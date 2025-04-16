using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("Audio Source")]
    public AudioSource sfxSource;  // 사운드 효과를 재생할 AudioSource

    [Header("Audio Clips")]
    public AudioClip shootSound;
    public AudioClip damageSound;
    public AudioClip deathSound;
    public AudioClip specialSkillSound;

    // SoundType 열거형: 각 상황에 따른 사운드를 구분합니다.
    public enum SoundType { Shoot, Damage, Death, SpecialSkill };

    void Awake()
    {
        // 싱글턴 패턴
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// SoundType에 따라 해당 AudioClip을 재생합니다.
    /// </summary>
    public void PlaySound(SoundType type)
    {
        if (sfxSource == null)
        {
            Debug.LogWarning("SoundManager: AudioSource가 할당되지 않았습니다!");
            return;
        }

        AudioClip clipToPlay = null;
        switch (type)
        {
            case SoundType.Shoot:
                clipToPlay = shootSound;
                break;
            case SoundType.Damage:
                clipToPlay = damageSound;
                break;
            case SoundType.Death:
                clipToPlay = deathSound;
                break;
            case SoundType.SpecialSkill:
                clipToPlay = specialSkillSound;
                break;
        }

        if (clipToPlay != null)
        {
            sfxSource.PlayOneShot(clipToPlay);
        }
    }
}
