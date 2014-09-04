using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AppRoot : MonoBehaviour 
{
	[SerializeField]
	RectTransform pnlTimer;

	[SerializeField]
	SettingsController pnlSettingsController;

	[SerializeField]
	RectTransform pnlWorkout;

	[SerializeField]
	RectTransform pnlMusic;

	[SerializeField]
	public Locker Locker;

	[SerializeField]
	TimerVisualController TimerController;


	[SerializeField]
	public ExcerciseDb ExcerciseDb;


	[SerializeField]
	public MusicDb MusicDb;

	[SerializeField]
	public AudioManager AudioManager;


	[SerializeField]
	public Timer Timer;

	public static AppRoot Instance { get { return _instance; } }

	private static AppRoot _instance = null;

	void Awake()
	{
		_instance = this;

		// run in 20 fps
		UnityEngine.Application.targetFrameRate = 20;
		UnityEngine.Application.runInBackground = true;

	}


	void Start () 
	{
		TimerController.TimerRef = Timer;

		Timer.Reset();

		Timer.IntervalTick += AudioManager.OnTick;
		Timer.IntervalStarted += AudioManager.OnIntervalStarted;
		Timer.TimerStarted += AudioManager.OnTimerStarted;
		Timer.TimerEnded += AudioManager.OnTimerEnded;
		Timer.TimerPause += AudioManager.OnTimerPause;
		Timer.TimerUnPause += AudioManager.OnTimerUnPause;
		Timer.TimerReset += AudioManager.OnTimerReset;


		Timer.TimerStarted += TimerController.OnTimerStarted;
		Timer.TimerEnded += TimerController.OnTimerEnded;
		Timer.TimerPause += TimerController.OnTimerPause;
		Timer.TimerUnPause += TimerController.OnTimerUnPause;
		Timer.TimerReset += TimerController.OnTimerReset;

		Timer.IntervalStarted += TimerController.OnIntervalStarted;

		string dataPath = UnityEngine.Application.dataPath;
	

		Debug.Log(dataPath);
	}
	
	void Update () 
	{
	}


	public void PauseTimer()
	{
		if (Timer.CurrentState == Timer.State.Running)
		{
			Timer.Pause();
		}
	}

	public void Stop()
	{
		Screen.sleepTimeout = SleepTimeout.SystemSetting;
		Timer.Reset();
	}


	public void Play()
	{
		if (Timer.CurrentState == Timer.State.Stopped)
		{
			Screen.sleepTimeout = SleepTimeout.NeverSleep;
			Timer.Run();
		}
		else if (Timer.CurrentState == Timer.State.Paused)
		{
			Screen.sleepTimeout = SleepTimeout.NeverSleep;
			Timer.UnPause();
		}
	}


	public void OnPresetClick(PresetController preset)
	{
		pnlSettingsController.OnPresetClick (preset);
	}

	public void ShowSettingsPage()
	{
		pnlTimer.gameObject.SetActive(false);
		pnlSettingsController.gameObject.SetActive(true);
		pnlSettingsController.Init();
	}

	public void ShowTimerPage()
	{
		pnlTimer.gameObject.SetActive(true);
		pnlSettingsController.gameObject.SetActive(false);
	}

	public void ShowPageWorkout()
	{
		pnlTimer.gameObject.SetActive(false);
		pnlSettingsController.gameObject.SetActive(false);
		pnlWorkout.gameObject.SetActive(true);
	}

	public void ShowPageMusic()
	{
		pnlTimer.gameObject.SetActive(false);
		pnlSettingsController.gameObject.SetActive(false);
		pnlWorkout.gameObject.SetActive(false);
		pnlMusic.gameObject.SetActive(true);
	}



}
