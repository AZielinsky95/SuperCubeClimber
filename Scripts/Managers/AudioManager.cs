using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

    [SerializeField] private AudioSource m_MusicSource;
    [SerializeField] private AudioSource m_SFXSource;
    [SerializeField] private AudioSource m_LoopingSFXSource;

    private bool m_IsMuted;
    private static AudioManager m_Instance;
    public static AudioManager Instance
    {
        get
        {
            if (m_Instance == null)
            {
                GameObject obj = new GameObject("AudioManager");
                m_Instance = obj.AddComponent<AudioManager>();
                m_Instance.m_MusicSource = obj.AddComponent<AudioSource>();
                m_Instance.m_SFXSource = obj.AddComponent<AudioSource>();
                m_Instance.m_LoopingSFXSource = obj.AddComponent<AudioSource>();
                DontDestroyOnLoad(obj);
            }

            return m_Instance;
        }
    }

    public bool IsMusicPlaying { get { return m_MusicSource.isPlaying; } }

    public bool IsSFXPlaying { get { return m_SFXSource.isPlaying; } }

    public bool IsMuted { get { return m_IsMuted; } } 

	void Awake()
	{
		if(m_Instance != null && m_Instance != this)
		{
			Destroy(gameObject);
		}
			
		m_Instance = this;

		DontDestroyOnLoad(gameObject);
	}

    public void MuteAudio()
    {
        m_MusicSource.mute = true;
        m_SFXSource.mute = true;
        m_LoopingSFXSource.mute = true;
        m_IsMuted = true;
    }

    public void UnMuteAudio()
    {
        m_MusicSource.mute = false;
        m_SFXSource.mute = false;
        m_LoopingSFXSource.mute = false;
        m_IsMuted = false;
    }

	public void PlaySFX(AudioClip sfx, float delay, float volume)
	{
		m_SFXSource.clip = sfx;
		m_SFXSource.PlayDelayed (delay);
	}

	public void PlayOneShotSFX(AudioClip sfx, float volume)
	{
		m_SFXSource.clip = sfx;
		m_SFXSource.PlayOneShot (sfx, volume);
	}

    public void PlaySFX(string sfx, float volume = 1)
	{
        AudioClip clip = Resources.Load<AudioClip>(sfx);
		m_SFXSource.clip = clip;
        m_SFXSource.volume = volume;
        m_SFXSource.Play();
	}

	public void PlaySFX(AudioClip sfx)
	{
		m_SFXSource.PlayOneShot (sfx);	
	}

	public void PlayLoopingSFX(AudioClip sfx,float delay = 0,float volume = 1)
	{
        m_LoopingSFXSource.clip = sfx;
        m_LoopingSFXSource.loop = true;
        m_LoopingSFXSource.volume = volume;
        m_LoopingSFXSource.PlayDelayed (delay);
	}

	public void StopSFX()
	{
		m_SFXSource.Stop();
        m_LoopingSFXSource.Stop();
    }

	public void PlayMusic(AudioClip music)
	{
		if (m_MusicSource != null) 
		{
			StopMusic();
		}

		m_MusicSource.clip = music;
        m_MusicSource.loop = true;
		m_MusicSource.Play();
	}

	public void PauseMusic()
	{
		m_MusicSource.Pause();
	}

	public void UnPauseMusic()
	{
		m_MusicSource.UnPause();
	}

	public void PlayMusic(string music)
	{
		m_MusicSource.Stop();
		m_MusicSource.clip.name = music;
		m_MusicSource.Play();
	}

	public void PlayMusic()
	{
		m_MusicSource.Play ();
	}

	public void StopMusic()
	{
		m_MusicSource.Stop ();
	}
		
}

