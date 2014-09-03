using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
	public enum TimerType
	{
		Main = 0,
		Interval0,
		Interval1
	}

	[SerializeField]
	GameObject presetPrefab;

	[SerializeField]
	Button btnHours0;

	[SerializeField]
	Button btnHours1;

	[SerializeField]
	Button btnMinutes0;
	
	[SerializeField]
	Button btnMinutes1;

	[SerializeField]
	Button btnSeconds0;
	
	[SerializeField]
	Button btnSeconds1;

	[SerializeField]
	Button btnRepetitions;

	[SerializeField]
	Button btnMinutesWarmUp;

	[SerializeField]
	Button btnSecondsWarmUp;



	[SerializeField]
	Text txtTime;


	Button TimeButton {get; set;}


	[SerializeField]
	Transform warmUpContainer;

	[SerializeField]
	Transform interval0Container;

	[SerializeField]
	Transform interval1Container;

	[SerializeField]
	Transform presetContainer;

	Dictionary<Button, Text>  ButtonTexts { get; set; } 




	// Use this for initialization
	void Awake ()
	{
		ButtonTexts = new Dictionary<Button, Text> ();

		ButtonTexts.Add (btnHours0, btnHours0.GetComponentInChildren<Text> ());
		ButtonTexts.Add (btnMinutes0, btnMinutes0.GetComponentInChildren<Text> ());
		ButtonTexts.Add (btnSeconds0, btnSeconds0.GetComponentInChildren<Text> ());

		ButtonTexts.Add (btnHours1, btnHours1.GetComponentInChildren<Text> ());
		ButtonTexts.Add (btnMinutes1, btnMinutes1.GetComponentInChildren<Text> ());
		ButtonTexts.Add (btnSeconds1, btnSeconds1.GetComponentInChildren<Text> ());

		ButtonTexts.Add (btnMinutesWarmUp, btnMinutesWarmUp.GetComponentInChildren<Text> ());
		ButtonTexts.Add (btnSecondsWarmUp, btnSecondsWarmUp.GetComponentInChildren<Text> ());


		ButtonTexts.Add (btnRepetitions, btnRepetitions.GetComponentInChildren<Text> ());


	}
	
	// Update is called once per frame
	void Update () 
	{
		txtTime.text = Application.Instance.Timer.DurationFormatted(false);

	}
	public void OnPresetClick(PresetController preset)
	{
		Application.Instance.Timer.SetPresetConfig(preset.PresetName);

		Init ();
	}

	
	public void Init()
	{
		warmUpContainer.gameObject.SetActive(false);
		interval0Container.gameObject.SetActive(false);
		interval1Container.gameObject.SetActive(false);

		if (Application.Instance.Timer.Cfg.Intervals.Count > 0)
		{
			warmUpContainer.gameObject.SetActive(true);
			ButtonTexts[btnMinutesWarmUp].text = Application.Instance.Timer.Cfg.Intervals[0].minutes.ToString();
			ButtonTexts[btnSecondsWarmUp].text = Application.Instance.Timer.Cfg.Intervals[0].seconds.ToString();
		}

		if (Application.Instance.Timer.Cfg.Intervals.Count > 1)
		{
			interval0Container.gameObject.SetActive(true);
			ButtonTexts[btnHours0].text = Application.Instance.Timer.Cfg.Intervals[1].hours.ToString();
			ButtonTexts[btnMinutes0].text = Application.Instance.Timer.Cfg.Intervals[1].minutes.ToString();
			ButtonTexts[btnSeconds0].text = Application.Instance.Timer.Cfg.Intervals[1].seconds.ToString();
		}

		if (Application.Instance.Timer.Cfg.Intervals.Count > 2)
		{
			interval1Container.gameObject.SetActive(true);
			ButtonTexts[btnHours1].text = Application.Instance.Timer.Cfg.Intervals[2].hours.ToString();
			ButtonTexts[btnMinutes1].text = Application.Instance.Timer.Cfg.Intervals[2].minutes.ToString();
			ButtonTexts[btnSeconds1].text = Application.Instance.Timer.Cfg.Intervals[2].seconds.ToString();
		}

		ButtonTexts[btnRepetitions].text = Application.Instance.Timer.Cfg.RepetitionCount.ToString();
	}


	private void FillPresets()
	{
		for (int i = 0; i < presetContainer.childCount; ++i)
		{
			Destroy(presetContainer.GetChild(i).gameObject);
		}


		for (int i = 0; i < Application.Instance.Timer.Presets.Count; ++i)
		{
			if (Application.Instance.Timer.Presets[i].Enabled)
			{
				GameObject presetGo = Instantiate(presetPrefab) as GameObject;
				presetGo.transform.parent = presetContainer;

				presetGo.transform.GetComponentInChildren<Text>().text = Application.Instance.Timer.Presets[i].Name;
				presetGo.name = Application.Instance.Timer.Presets[i].Name;

				presetGo.transform.GetComponent<PresetController>().PresetName = Application.Instance.Timer.Presets[i].Name;
			}
		}
	}






	public void OnWarmUpMinutes(Button button)
	{
		Application.Instance.Timer.Cfg.Intervals [0].minutes += button.name.Contains("Plus") ? 1 : -1;
		Application.Instance.Timer.Cfg.Intervals [0].minutes = Mathf.Clamp (Application.Instance.Timer.Cfg.Intervals [0].minutes, 0, 59);
		Init ();
	}

	public void OnWarmUpSeconds(Button button)
	{
		Application.Instance.Timer.Cfg.Intervals [0].seconds += button.name.Contains("Plus") ? 1 : -1;
		Application.Instance.Timer.Cfg.Intervals [0].seconds = Mathf.Clamp (Application.Instance.Timer.Cfg.Intervals [0].seconds, 0, 59);
		Init ();

	}



	public void OnInterval0Hours (Button button)
	{
		Application.Instance.Timer.Cfg.Intervals [1].hours += button.name.Contains("Plus") ? 1 : -1;
		Application.Instance.Timer.Cfg.Intervals [1].hours = Mathf.Clamp (Application.Instance.Timer.Cfg.Intervals [1].hours, 0, 99);
		Init ();
	}

	public void OnInterval0Minutes (Button button)
	{
		Application.Instance.Timer.Cfg.Intervals [1].minutes += button.name.Contains("Plus") ? 1 : -1;;
		Application.Instance.Timer.Cfg.Intervals [1].minutes = Mathf.Clamp (Application.Instance.Timer.Cfg.Intervals [1].minutes, 0, 59);
		Init ();
	}

	public void OnInterval0Seconds (Button button)
	{
		Application.Instance.Timer.Cfg.Intervals [1].seconds += button.name.Contains("Plus") ? 1 : -1;;
		Application.Instance.Timer.Cfg.Intervals [1].seconds = Mathf.Clamp (Application.Instance.Timer.Cfg.Intervals [1].seconds, 0, 59);
		Init ();
	}

	public void OnInterval1Hours (Button button)
	{
		Application.Instance.Timer.Cfg.Intervals [2].hours += button.name.Contains("Plus") ? 1 : -1;
		Application.Instance.Timer.Cfg.Intervals [2].hours = Mathf.Clamp (Application.Instance.Timer.Cfg.Intervals [2].hours, 0, 99);
		Init ();
	}
	
	public void OnInterval1Minutes (Button button)
	{
		Application.Instance.Timer.Cfg.Intervals [2].minutes += button.name.Contains("Plus") ? 1 : -1;
		Application.Instance.Timer.Cfg.Intervals [2].minutes = Mathf.Clamp (Application.Instance.Timer.Cfg.Intervals [2].minutes, 0, 59);
		Init ();
	}
	
	public void OnInterval1Seconds (Button button)
	{
		Application.Instance.Timer.Cfg.Intervals [2].seconds += button.name.Contains("Plus") ? 1 : -1;
		Application.Instance.Timer.Cfg.Intervals [2].seconds = Mathf.Clamp (Application.Instance.Timer.Cfg.Intervals [2].seconds, 0, 59);
		Init ();
	}

	public void OnRounds (Button button)
	{
		Application.Instance.Timer.Cfg.RepetitionCount += button.name.Contains("Plus") ? 1 : -1;
		Application.Instance.Timer.Cfg.RepetitionCount = Mathf.Clamp (Application.Instance.Timer.Cfg.RepetitionCount, 1, 99);
		Init ();
	}

	void OnEnable()
	{
		Init();
	
		FillPresets ();
	}

	public void OnBack()
	{

		Application.Instance.ShowTimerPage();
	}


}
