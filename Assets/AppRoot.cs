using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AppRoot : core.AppRootBase 
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

	protected override void OnPreInit()
	{
		_instance = this;

		// run in 20 fps
		UnityEngine.Application.targetFrameRate = 20;
		UnityEngine.Application.runInBackground = true;
	}


	protected override void OnPostInit()
	{
		TimerController.TimerRef = Timer;

		Timer.Reset();

		Timer.TimerEnded += this.OnTimerEnded;

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
		if (Application.platform == RuntimePlatform.Android)
		{
			if (Input.GetKey(KeyCode.Escape))
			{
				Application.Quit();
				return;
			}
		}
	}


	public void PauseTimer()
	{
		if (Timer.CurrentState == Timer.State.Running)
		{
			Timer.Pause();
			CrittercismAndroid.LeaveBreadcrumb("AppRoot.PauseTimer() ");
		}
		else
		{
			
		}
	}

	public void Stop(bool showConfirm)
	{
		if (showConfirm && Timer.CurrentState != Timer.State.Stopped)
		{
			PauseTimer();
			UIManager.OpenDialog(trn.ui.DialogDef.DlgResetTimer);
		
		}
		else
		{
			Screen.sleepTimeout = SleepTimeout.SystemSetting;
			Timer.Reset();

			CrittercismAndroid.LeaveBreadcrumb("AppRoot.Stop()");
		}
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

		CrittercismAndroid.LeaveBreadcrumb("AppRoot.Play()");
	}


	public void OnPresetClick(PresetController preset)
	{
		pnlSettingsController.OnPresetClick (preset);

		CrittercismAndroid.LeaveBreadcrumb("AppRoot.OnPresetClick() " + preset.PresetName);
	}

	public void OnTimerEnded()
	{
		UIManager.OpenDialog (trn.ui.DialogDef.DlgTrainingFinished);
	}

	public void ShowSettingsPage()
	{
		pnlTimer.gameObject.SetActive(false);
		pnlSettingsController.gameObject.SetActive(true);
		pnlSettingsController.Init();

		CrittercismAndroid.LeaveBreadcrumb("AppRoot.ShowSettingsPage() ");
	}

	public void ShowTimerPage()
	{
		pnlTimer.gameObject.SetActive(true);
		pnlSettingsController.gameObject.SetActive(false);

		CrittercismAndroid.LeaveBreadcrumb("AppRoot.ShowTimerPage() ");
	}

	public void ShowPageWorkout()
	{
		pnlTimer.gameObject.SetActive(false);
		pnlSettingsController.gameObject.SetActive(false);
		pnlWorkout.gameObject.SetActive(true);

		CrittercismAndroid.LeaveBreadcrumb("AppRoot.ShowPageWorkout() ");
	}

	public void ShowPageMusic()
	{
		pnlTimer.gameObject.SetActive(false);
		pnlSettingsController.gameObject.SetActive(false);
		pnlWorkout.gameObject.SetActive(false);
		pnlMusic.gameObject.SetActive(true);

		CrittercismAndroid.LeaveBreadcrumb("AppRoot.ShowPageMusic() ");
	}


	public void OnCancelWorkoutDialogOK()
	{
		UIManager.CloseDialog (trn.ui.DialogDef.DlgResetTimer);

		Stop (false);
	}

	public void OnCancelWorkoutDialogBack()
	{
		core.dbg.Dbg.Assert(Timer.CurrentState == Timer.State.Paused, "AppRoot.OnCancelWorkoutDialogBack unexpected timer state");

		UIManager.CloseDialog (trn.ui.DialogDef.DlgResetTimer);

		Play();
	}

	public void OnTrainingFinishedDialogOK()
	{
		UIManager.CloseDialog (trn.ui.DialogDef.DlgTrainingFinished);
		Timer.Reset ();
	}

	public void OnDemoVersionDialogPurchase()
	{
		Utils.Instance.OpenFullVersionLink ();
	}

	public void OnDemoVersionContinueDemo()
	{
		UIManager.CloseDialog (trn.ui.DialogDef.DlgDemoVersion);
	}


}
