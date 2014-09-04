using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IAudioManager
{
	void Play();
	void Stop();
	void Pause();
	void UnPause();
	void RewindSong(float time);
	void SetMusic(MusicDb.MusicGroup musicGroup);


	bool IsPlaying();
	bool IsMusicSet();

	float GetSongTime();
	float GetSongLength();


	void OnTick(bool last);
	void OnIntervalStarted(Timer.IntervalDefinition intervalDef);
	void OnTimerStarted(Timer.Config config);
	void OnTimerEnded();
	void OnTimerPause(Timer.IntervalDefinition intervalDef);
	void OnTimerUnPause(Timer.IntervalDefinition intervalDef);
	void OnTimerReset();
	

}

public class AudioManager : MonoBehaviour, IAudioManager
{
	[SerializeField]
	AudioSource MusicAudioSource;

	[SerializeField]
	AudioSource EffectAudioSource; 


	enum State
	{
		Stopped = 0,
		Playing,
		Paused
	}

	State CurrentState { get; set; }

	float DstMusicVolume { get; set; }


	MusicDb.MusicGroup actualMusicGroup { get; set; } 
	private int songIndex{ get; set; }

	private Timer.Config TimerConfig { get; set; }

	void Awake()
	{}

	void Start()
	{
		CurrentState = State.Stopped;
	}


	public void SetMusic(MusicDb.MusicGroup musicGroup)
	{
		actualMusicGroup = musicGroup;
		songIndex = 0;
	}

	public bool IsPlaying()
	{
		return IsMusicSet() && MusicAudioSource.isPlaying;
	}


	public bool IsMusicSet()
	{
		return actualMusicGroup != null;
	}

	public float GetSongLength()
	{
		float len = 0;
		if (MusicAudioSource.clip != null)
		{
			len = MusicAudioSource.clip.length;
		}
	
		return len;
	}

	public float GetSongTime()
	{
		float time = -1;
		if (IsMusicSet())
		{
			time = MusicAudioSource.time;
		}

		return time;
	}



	public delegate void SongEnded();

	AudioClip LoadAudioClip()
	{
		return Resources.Load("Music/" + actualMusicGroup.name +"/"+actualMusicGroup.ClipNameList[songIndex]) as AudioClip;
	}

	public void Play()
	{
		if (IsMusicSet())
		{
			MusicAudioSource.clip = LoadAudioClip();
			MusicAudioSource.Play();

			CurrentState = State.Playing;
		}
	}

	public void Pause()
	{
		MusicAudioSource.Pause();

		CurrentState = State.Paused;
	}

	public void UnPause()
	{
		MusicAudioSource.Play();
		
		CurrentState = State.Playing;
	}

	public void Stop()
	{
		MusicAudioSource.Stop();
			
		CurrentState = State.Stopped;
	}

	private void OnSongEnded()
	{
		if (IsMusicSet())
		{
			songIndex++;
			if (songIndex < actualMusicGroup.ClipNameList.Count)
			{
				Play();
			}
			else
			{
				if (TimerConfig != null && TimerConfig.LoopMusic)
				{
					songIndex = 0;
					Play();
				}
				else
				{
					Stop ();
				}
			}
		}
	}


	public void RewindSong(float time)
	{
		if (IsMusicSet())
		{
			float currentTime = GetSongTime();

			float newTime = currentTime + time;

			if (newTime < 0)
				newTime = 0;
			else if (newTime > GetSongLength())
				newTime = GetSongLength();

			SetSongTime(newTime);
		
		}
	}

	public void SetSongTime(float time)
	{
		if (IsMusicSet())
		{
			MusicAudioSource.time = time;
		}
	}



	void Update()
	{
		MusicAudioSource.volume = Mathf.Lerp(MusicAudioSource.volume, DstMusicVolume, Time.deltaTime * 5.0f);

		if (CurrentState == State.Playing && !MusicAudioSource.isPlaying)
		{
			OnSongEnded();
		}
	}


	////   Delegates   ////
	public void OnTick(bool last)
	{
		MusicDb.EffectCategoryType tick = last ? MusicDb.EffectCategoryType.TickLong : MusicDb.EffectCategoryType.Tick;

		EffectAudioSource.clip = AppRoot.Instance.MusicDb.GetEffect(tick);
		EffectAudioSource.Play();
	}

	public void OnIntervalStarted(Timer.IntervalDefinition intervalDef)
	{
		DstMusicVolume = intervalDef.MusicVolume;
	}

	public void OnTimerStarted(Timer.Config config)
	{
		TimerConfig = config;

		songIndex = 0; // start from first song
	
		if (config.MusicName != "")
		{
			SetMusic(AppRoot.Instance.MusicDb.GetMusicGroup(config.MusicName));
		}
		Play();
	}

	public void OnTimerEnded()
	{
		EffectAudioSource.clip = AppRoot.Instance.MusicDb.GetEffect(MusicDb.EffectCategoryType.TimerFinish);
		EffectAudioSource.Play();

		Pause();
	}

	public void OnTimerPause(Timer.IntervalDefinition intervalDef)
	{
		Pause();
	}

	public void OnTimerUnPause(Timer.IntervalDefinition intervalDef)
	{
		DstMusicVolume = intervalDef.MusicVolume;

		UnPause();
	}

	public void OnTimerReset()
	{
		Stop();
	}

}
