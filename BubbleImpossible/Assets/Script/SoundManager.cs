using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    // BGM 종류
    public enum EBgm
    {
        BGM_TITLE,
        BGM_GAME,
    }

    // SFX 종류
    public enum ESfx
    {
        // 플레이어 관련
        SFX_SHOOT,        // 0. 총알 발사
        SFX_DAMAGE,       // 1. 데미지
        SFX_DEATH,        // 2. 사망
        SFX_SPECIALSKILL, // 3. 특수 스킬

        //보스 관련
        SFX_BOSS_SHOOT, // 4. 보스 공격
        SFX_BOSS_HIT,   // 5. 보스 데미지
        SFX_EXPLOSION,  // 6. 보스 죽음
        SFX_TIMER_END,  // 7. 타임 오버
    }

    [Header("Audio Clips")]
    [SerializeField] private AudioClip[] bgms;
    [SerializeField] private AudioClip[] sfxs;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource audioBgm;
    [SerializeField] private AudioSource audioSfx;

    private void Awake()
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

    /// <summary>지정한 BGM을 재생합니다.</summary>
    public void PlayBGM(EBgm bgmIdx)
    {
        if (audioBgm == null || bgms == null || (int)bgmIdx >= bgms.Length) return;
        audioBgm.clip = bgms[(int)bgmIdx];
        audioBgm.loop = true;
        audioBgm.Play();
    }

    /// <summary>현재 재생 중인 BGM을 중지합니다.</summary>
    public void StopBGM()
    {
        if (audioBgm == null) return;
        audioBgm.Stop();
    }

    /// <summary>지정한 SFX를 한 번 재생합니다.</summary>
    public void PlaySFX(ESfx sfxIdx)
    {
        if (audioSfx == null || sfxs == null || (int)sfxIdx >= sfxs.Length) return;
        audioSfx.PlayOneShot(sfxs[(int)sfxIdx]);
    }
}
