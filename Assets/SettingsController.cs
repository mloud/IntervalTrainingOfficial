using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
	public enum NumericType
	{
		Rounds,
		WarmUp,
		Work,
		Rest
	}

	[SerializeField]
	GameObject presetPrefab;

	[System.Serializable]
	private class NumericClass
	{
		public NumericType Type;

		public List<NumItem> NumItems; 
	}


	[SerializeField]
	List<NumericClass> _numericObjects;


	[SerializeField]
	Text txtTime;


	Button TimeButton {get; set;}

	[SerializeField]
	Transform presetContainer;






	// Use this for initialization
	void Awake ()
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		txtTime.text = AppRoot.Instance.Timer.DurationFormatted(false);

	}
	public void OnPresetClick(PresetController preset)
	{
		AppRoot.Instance.Timer.SetPresetConfig(preset.PresetName);

		Init ();
	}

	
	public void Init()
	{
		ShowNumericObjects (true);

	
		if (AppRoot.Instance.Timer.Cfg.Intervals.Count > 0)
		{
			NumericClass numObj = _numericObjects.Find(x=>x.Type == NumericType.WarmUp);

			numObj.NumItems[0].SetValue(AppRoot.Instance.Timer.Cfg.Intervals[0].minutes);
			numObj.NumItems[1].SetValue(AppRoot.Instance.Timer.Cfg.Intervals[0].seconds);

			numObj.NumItems[0].OnValueChanged = OnWarmUpMinutes;
			numObj.NumItems[1].OnValueChanged = OnWarmUpSeconds;

		}

		if (AppRoot.Instance.Timer.Cfg.Intervals.Count > 1)
		{
			NumericClass numObj = _numericObjects.Find(x=>x.Type == NumericType.Work);
				
			numObj.NumItems[0].SetValue(AppRoot.Instance.Timer.Cfg.Intervals[1].minutes);
			numObj.NumItems[1].SetValue(AppRoot.Instance.Timer.Cfg.Intervals[1].seconds);

			numObj.NumItems[0].OnValueChanged = OnWorkMinutes;
			numObj.NumItems[1].OnValueChanged = OnWorkSeconds;
		}

		if (AppRoot.Instance.Timer.Cfg.Intervals.Count > 2)
		{
			NumericClass numObj = _numericObjects.Find(x=>x.Type == NumericType.Rest);
			
			numObj.NumItems[0].SetValue(AppRoot.Instance.Timer.Cfg.Intervals[2].minutes);
			numObj.NumItems[1].SetValue(AppRoot.Instance.Timer.Cfg.Intervals[2].seconds);

			numObj.NumItems[0].OnValueChanged = OnRestMinutes;
			numObj.NumItems[1].OnValueChanged = OnRestSeconds;
		}

		NumericClass numObjRounds = _numericObjects.Find(x=>x.Type == NumericType.Rounds);

		numObjRounds.NumItems[0].SetValue(AppRoot.Instance.Timer.Cfg.RepetitionCount);
		numObjRounds.NumItems[0].OnValueChanged = OnRounds;


	}

	private void ShowNumericObjects(bool show)
	{
		for (int i = 0; i < _numericObjects.Count; ++i)
		{
			for (int j = 0; j < _numericObjects[i].NumItems.Count; ++j)
			{
				_numericObjects[i].NumItems[j].gameObject.SetActive(show);
			}
		}
	}

	private void FillPresets()
	{
		for (int i = 0; i < presetContainer.childCount; ++i)
		{
			Destroy(presetContainer.GetChild(i).gameObject);
		}


		for (int i = 0; i < AppRoot.Instance.Timer.Presets.Count; ++i)
		{
			if (AppRoot.Instance.Timer.Presets[i].Enabled)
			{
				GameObject presetGo = Instantiate(presetPrefab) as GameObject;
				presetGo.transform.parent = presetContainer;

				PresetController presetController = presetGo.transform.GetComponent<PresetController>();

				presetController.ExtText.SetTextKey(AppRoot.Instance.Timer.Presets[i].Name);
				presetController.PresetName = AppRoot.Instance.Timer.Presets[i].Name;
				presetController.name = AppRoot.Instance.Timer.Presets[i].Name;

			}
		}
	}






	public void OnWarmUpMinutes(NumItem numItem)
	{
		AppRoot.Instance.Timer.Cfg.Intervals [0].minutes = numItem.Value;
	}

	public void OnWarmUpSeconds(NumItem numItem)
	{
		AppRoot.Instance.Timer.Cfg.Intervals [0].seconds = numItem.Value;
	}


	public void OnWorkMinutes (NumItem numItem)
	{
		AppRoot.Instance.Timer.Cfg.Intervals [1].minutes = numItem.Value;
	}

	public void OnWorkSeconds (NumItem numItem)
	{
		AppRoot.Instance.Timer.Cfg.Intervals [1].seconds = numItem.Value;
	}

	public void OnRestMinutes (NumItem numItem)
	{
		AppRoot.Instance.Timer.Cfg.Intervals [2].minutes = numItem.Value;
	}
	
	public void OnRestSeconds (NumItem numItem)
	{
		AppRoot.Instance.Timer.Cfg.Intervals [2].seconds = numItem.Value;
	}

	public void OnRounds (NumItem numItem)
	{
		AppRoot.Instance.Timer.Cfg.RepetitionCount = numItem.Value;
	}

	void OnEnable()
	{
		Init();
	
		FillPresets ();
	}

	void OnDisable()
	{
		ShowNumericObjects (false);
	}

	public void OnBack()
	{

		AppRoot.Instance.ShowTimerPage();
	}


	private void UpdateInput()
	{
	}

}
