using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public class TimerVisualController : MonoBehaviour {

	[SerializeField]
	Text lblTimeElapsed;

	[SerializeField]
	Text lblTimeDuration;

	[SerializeField]
	Text lblTimeRest;

	[SerializeField]
	Text lblIntervalDuration;

	[SerializeField]
	Text lblIntervalElapsed;

	[SerializeField]
	Text lblIntervalRest;

	[SerializeField]
	Text lblActualTime;

	[SerializeField]
	Button btnPlay;

	[SerializeField]
	Button btnPause;


	[SerializeField]
	Text txtRound;

	[SerializeField]
	Text txtRoundCount;

	[SerializeField]
	ProgressBar progBarMainTimer;

	[SerializeField]
	Text txtPaused;

	[SerializeField]
	ProgressBar progBarIntervalTimer;

	[SerializeField]
	Image imgIntervalImage;

	[SerializeField]
	core.ui.ExtText intervalInfoText;


	private Color InactiveIntervalImageColor;
	private float DstPausedColorAlpha;

	private float RefreshTimer { get; set; }

	public Timer TimerRef { get; set; }

	void Awake()
	{
		txtPaused.gameObject.SetActive (false);
	}

	void Start ()
	{
		InactiveIntervalImageColor = imgIntervalImage.color;

	}


	void SetTimeTextInLabel(Text label, string text)
	{
//		float scale = 1.0f;
//
//		if (text.Length > 9)
//		{
//			scale = 0.5f;
//		}
//		else if (text.Length > 6)
//		{
//			scale = 0.7f;
//		}
//
//
//		label.transform.localScale = new Vector3(scale, scale,scale);
		label.text = text;
	}

	private void UpdateTexts()
	{
		if (TimerRef != null) 
		{
				SetTimeTextInLabel (lblIntervalDuration, TimerRef.CurrentIntervalDurationFormatted (false));
				//SetTimeTextInLabel (lblIntervalElapsed, TimerRef.CurrentIntervalElapsedTimeFormatted (false));
				SetTimeTextInLabel (lblIntervalRest, TimerRef.CurrentIntervalRestTimeFormatted (false));
		
				//SetTimeTextInLabel (lblTimeElapsed, TimerRef.TotalElapsedTimeFormatted (false));
				SetTimeTextInLabel (lblTimeDuration, TimerRef.TotalDurationFormatted (false));
				SetTimeTextInLabel (lblTimeRest, TimerRef.TotalRestTimeFormatted (false));
		
		
				txtRound.text = TimerRef.CurrentRound ().ToString ();
				txtRoundCount.text = TimerRef.RoundCount ().ToString ();
				
				lblActualTime.text = TimerRef.GetActualTime();
		}
	}

	void Update () 
	{
		if (TimerRef != null)
		{

			if (Time.time > RefreshTimer) 
			{
				UpdateTexts();
				RefreshTimer = Time.time + 1.0f;
			}

		
			progBarMainTimer.Set (1 - TimerRef.TotalElapsedTime() / TimerRef.Duration());
			progBarIntervalTimer.Set (1 - TimerRef.CurrentIntervalElapsedTime() / TimerRef.CurrentIntervalDuration());
			

			
			// timer is running
			if (TimerRef.CurrentState == Timer.State.Running)
			{
				Color color = Col.ColorManager.Instance.GetColor(TimerRef.Cfg.Intervals[TimerRef.CurrentIntervalState.Index].ColorType);
				imgIntervalImage.color = color;
			}
			// timer paused - blinking
			else if (TimerRef.CurrentState == Timer.State.Paused)
			{

				float newAlpha = imgIntervalImage.color.a + ((imgIntervalImage.color.a - DstPausedColorAlpha) > 0 ? -0.035f : 0.035f);
			

				if ( Mathf.Abs(newAlpha - DstPausedColorAlpha) < 0.05f)
				{
					newAlpha = DstPausedColorAlpha;

					DstPausedColorAlpha = newAlpha == 0.2f ? 1 : 0.2f;
				}

				Color newColor = imgIntervalImage.color;
				newColor.a = newAlpha;
				imgIntervalImage.color = newColor;
			}
			// stopped - gray by default
			else
			{
				imgIntervalImage.color = InactiveIntervalImageColor;
			}
		}
	}

	public void OnTimerStarted(Timer.Config config)
	{
		btnPlay.gameObject.SetActive(false);
		btnPause.gameObject.SetActive(true);

		txtPaused.gameObject.SetActive (false);
		intervalInfoText.gameObject.SetActive (true);

		UpdateTexts ();
	}
	
	public void OnTimerEnded()
	{
		btnPlay.gameObject.SetActive(true);
		btnPause.gameObject.SetActive(false);
		intervalInfoText.gameObject.SetActive (false);
		UpdateTexts ();
	}
	
	public void OnTimerPause(Timer.IntervalDefinition intervalDef)
	{
		btnPlay.gameObject.SetActive(true);
		btnPause.gameObject.SetActive(false);
		intervalInfoText.gameObject.SetActive (false);
		DstPausedColorAlpha = 0.2f;

		txtPaused.gameObject.SetActive (true);


		UpdateTexts ();

	}

	public void OnIntervalStarted(Timer.IntervalDefinition intervalDef)
	{
		intervalInfoText.gameObject.SetActive (true);
		intervalInfoText.SetTextKey (intervalDef.InfoText);

		txtPaused.gameObject.SetActive (false);

	}
	
	public void OnTimerUnPause(Timer.IntervalDefinition intervalDef)
	{
		btnPlay.gameObject.SetActive(false);
		btnPause.gameObject.SetActive(true);
		intervalInfoText.gameObject.SetActive (true);
		// set opaque
		Color intervalColor = imgIntervalImage.color;
		intervalColor.a = 1;

		txtPaused.gameObject.SetActive (false);
		intervalInfoText.gameObject.SetActive (true);
		UpdateTexts ();
	
	}
	
	public void OnTimerReset()
	{
		btnPlay.gameObject.SetActive(true);
		btnPause.gameObject.SetActive(false);
		txtPaused.gameObject.SetActive (false);

		intervalInfoText.gameObject.SetActive (false);

		UpdateTexts ();
	}

}
