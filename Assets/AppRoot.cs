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
	public Button DemoVersionButton;

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
		UnityEngine.Application.targetFrameRate = 30;
		//UnityEngine.Application.runInBackground = true;

		Save = new trn.CustomSave ();

		(Save as trn.CustomSave).LoadPresets ();

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
	


		if (core.Config.Demo.Enabled)
		{
			UIManager.OpenDialog(trn.ui.DialogDef.DlgDemoVersion);
		}
		else
		{
			DemoVersionButton.gameObject.SetActive(false);
		}

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


	private bool CheckDemoRestriction(/*Timer.Config originalConfig*/)
	{
		if (core.Config.Demo.Enabled)
		{
			if (AppRoot.Instance.Timer.Duration() > core.Config.Demo.TimeLimit)
			{
				//AppRoot.Instance.Timer.Cfg = originalConfig;
				
				// todo show dialog
				AppRoot.Instance.UIManager.OpenDialog (trn.ui.DialogDef.DlgDemoVersion);

				return false;
			}
		}


		return true;
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
			Timer.UnPause();
			Screen.sleepTimeout = SleepTimeout.SystemSetting;
			Timer.Reset();

			CrittercismAndroid.LeaveBreadcrumb("AppRoot.Stop()");
		}
	}


	public void Play()
	{
		if (Timer.CurrentState == Timer.State.Stopped)
		{

			if (CheckDemoRestriction())
			{
				Screen.sleepTimeout = SleepTimeout.NeverSleep;
				Timer.Run();
			}
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

	public void OnDemoButtonClick()
	{
		if (Timer.CurrentState == Timer.State.Running)
		{
			PauseTimer();
		}
		UIManager.OpenDialog (trn.ui.DialogDef.DlgDemoVersion);
	}

	public void OnDemoVersionContinueDemo()
	{
		UIManager.CloseDialog (trn.ui.DialogDef.DlgDemoVersion);
	}

	public void OnRemovePreset(PresetController presetController)
	{
		core.ui.Dialog dialog = AppRoot.Instance.UIManager.OpenDialog (trn.ui.DialogDef.DlgRemovePreset);
		dialog.Paramater = presetController;
	}

	public void OnRemovePresetConfirmationYes(core.ui.Dialog dialog)
	{
		AppRoot.Instance.UIManager.CloseDialog (trn.ui.DialogDef.DlgRemovePreset);

		PresetController presetController = dialog.Paramater as PresetController;

		Destroy (presetController.gameObject);


		// set as not enabled
		int index = Timer.Presets.FindIndex (x => x.Name == presetController.name);

		if (index != -1)
		{
			Timer.Presets.RemoveAt(index);
		}

		(Save as trn.CustomSave).SavePresets ();

		pnlSettingsController.OnRemovePreset (presetController);
	}

	public void OnRemovePresetConfirmationNo()
	{
		AppRoot.Instance.UIManager.CloseDialog (trn.ui.DialogDef.DlgRemovePreset);
	}


	public void OnConfirmPreset(PresetController presetController)
	{
		Timer.Config config = Timer.Presets.Find (x => x.Name == presetController.name);

		config.SetFrom (Timer.Cfg);

		(Save as trn.CustomSave).SavePresets ();

		pnlSettingsController.OnConfirmPreset (presetController);

	}


	public void OnTrainingNameConfirm(Text textWithName)
	{

		// already exists
		if (Timer.Presets.Find(x=>x.Name == textWithName.text) != null)
		{
			UIManager.OpenDialog(trn.ui.DialogDef.DlgTrainingAlreadyExists);
		}
		// add
		else
		{
			UIManager.CloseDialog (trn.ui.DialogDef.DlgTrainingName);

			pnlSettingsController.AddNewPreset(textWithName.text);

			(Save as trn.CustomSave).SavePresets();
		}
	}

	public void OnTrainingAlreadyExist()
	{
		UIManager.CloseDialog (trn.ui.DialogDef.DlgTrainingAlreadyExists);
	}

	public void OnTrainingNameCancel()
	{
		UIManager.CloseDialog (trn.ui.DialogDef.DlgTrainingName);
	}

	
	protected override void OnPause()
	{
		if (Timer.CurrentState == Timer.State.Running)
		{
			PauseTimer();
		}

	}
	
}