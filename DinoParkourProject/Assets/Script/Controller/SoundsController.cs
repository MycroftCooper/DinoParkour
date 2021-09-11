using UnityEngine;

public class SoundsController : MonoBehaviour
{
    public GameObject BGMPlayer;
    public GameObject VoicePlayer;
    public GameObject SoundEffectsPlayer;

    private AudioSource bgmSource;
    private AudioSource voiceSource;
    private AudioSource soundEffectsSource;

    private void Awake()
    {
        if (BGMPlayer != null)
        {
            bgmSource = BGMPlayer.GetComponent<AudioSource>();
            bgmSource.volume = 1f;
        }
        if (VoicePlayer != null)
            voiceSource = VoicePlayer.GetComponent<AudioSource>();
        if (SoundEffectsPlayer != null)
            soundEffectsSource = SoundEffectsPlayer.GetComponent<AudioSource>();
        isMute = false;
    }

    private bool isMute;
    public bool IsMute
    {
        set
        {
            isMute = value;
            if (isMute)
                mute();
            else
                unmute();
        }
        get { return isMute; }
    }
    private void mute()
    {
        if (BGMPlayer != null)
            bgmSource.mute = true;
        if (VoicePlayer != null)
            voiceSource.mute = true;
        if (SoundEffectsPlayer != null)
            soundEffectsSource.mute = true;
        isMute = true;
    }
    private void unmute()
    {
        if (BGMPlayer != null)
            bgmSource.mute = false;
        if (VoicePlayer != null)
            voiceSource.mute = false;
        if (SoundEffectsPlayer != null)
            soundEffectsSource.mute = false;
        isMute = false;
    }
    public bool isBGMClipSetted() => bgmSource.clip != null;
    public void setBGMClip(string BGMPath)
    {
        if (bgmSource.isPlaying)
            bgmSource.Stop();
        AudioClip bgmClip = Resources.Load<AudioClip>(BGMPath);
        bgmSource.clip = bgmClip;
        bgmSource.loop = true;
    }
    public void PlayBGM()
    {
        if (bgmSource.isPlaying)
            bgmSource.Stop();
        bgmSource.Play();
    }
    public void PauseBGM()
    {
        if (bgmSource.isPlaying)
            bgmSource.Pause();
    }

    public void PlayVoice(string VoicePath)
    {
        AudioClip voiceClip = Resources.Load<AudioClip>(VoicePath);
        voiceSource.PlayOneShot(voiceClip);
    }
    public void PlaySoundEffect(string SoundEffectPath)
    {
        AudioClip soundEffectClip = Resources.Load<AudioClip>(SoundEffectPath);
        soundEffectsSource.PlayOneShot(soundEffectClip);
    }
}
