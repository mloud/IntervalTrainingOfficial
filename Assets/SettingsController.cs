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

	[SerializeField]
	GameObject presetAddNewPrefab;

	[SerializeField]
	Scrollbar presetScrollBar;

	[SerializeField]
	Toggle soundToggle;

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
	RectTransform presetContainer;


	private PresetController SelectedPreset { get; set; }

	private bool InitInprogress { get; set; }

	// Use this for initialization
	void Awake ()
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		txtTime.text = AppRoot.Instance.Timer.TotalDurationFormatted(false);
	
		//Debug.Log ("SEL: " + (SelectedPreset == null ? "null" : SelectedPreset.name));
	
	}

	public void AddNewPreset(string name)
	{
		core.dbg.Dbg.Log ("SettingsController.AddNewPreset() " + name);

		/*PresetController presetController = */ CreateNewPreset(name);

		FillPresets();


		OnPresetClick (presetContainer.FindChild (name).GetComponent<PresetController> ());
		//presetController.OnClick ();
	}

	public void OnPresetClick(PresetController preset)
	{
		DeselectAll ();

		if (preset.PresetName == "ADD_NEW")
		{
			AppRoot.Instance.UIManager.OpenDialog(trn.ui.DialogDef.DlgTrainingName);
		}
		else
		{
			AppRoot.Instance.Timer.SetPresetConfig(preset.PresetName);
			
			Init ();

			SelectPreset (preset);
		}
	}


	public void Init()
	{

		InitInprogress = true;

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


		// sound
		soundToggle.isOn = AppRoot.Instance.Timer.Cfg.Sound;

		InitInprogress = false;
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
				presetController.Removable = AppRoot.Instance.Timer.Presets[i].Removable;
			}
		}

		{
			// add new one
			GameObject presetGo = Instantiate(presetAddNewPrefab) as GameObject;
			presetGo.transform.parent = presetContainer;
			
			PresetController presetController = presetGo.transform.GetComponent<PresetController>();
			
			presetController.ExtText.SetTextKey("STR_ADD_NEW");
			presetController.PresetName = "ADD_NEW";
			presetController.name = "ADD_NEW";
			presetController.Removable = false;
		}

		ReinitPresetContainer ();
	}
	
	private void ReinitPresetContainer()
	{
		presetContainer.rect.Set (0, 0, 0, 0);
		presetContainer.sizeDelta = new Vector2((presetContainer.childCount) * 50.0f, presetContainer.sizeDelta.y * 0.5f);
	}
	

	public void OnWarmUpMinutes(NumItem numItem)
	{
		Timer.Config config = AppRoot.Instance.Timer.Cfg.Clone ();
		AppRoot.Instance.Timer.Cfg.Intervals [0].minutes = numItem.Value;

		if (SelectedPreset)
			SelectedPreset.ShowEditButton ();

		//CheckDemo (config);
	}

	public void OnWarmUpSeconds(NumItem numItem)
	{
		Timer.Config config = AppRoot.Instance.Timer.Cfg.Clone ();

		AppRoot.Instance.Timer.Cfg.Intervals [0].seconds = numItem.Value;


		if (SelectedPreset)
			SelectedPreset.ShowEditButton ();

		//CheckDemo (config);
	}


	public void OnWorkMinutes (NumItem numItem)
	{
		Timer.Config config = AppRoot.Instance.Timer.Cfg.Clone ();

		AppRoot.Instance.Timer.Cfg.Intervals [1].minutes = numItem.Value;


		if (SelectedPreset)
			SelectedPreset.ShowEditButton ();

		//CheckDemo (config);
	}

	public void OnWorkSeconds (NumItem numItem)
	{
		Timer.Config config = AppRoot.Instance.Timer.Cfg.Clone ();

		AppRoot.Instance.Timer.Cfg.Intervals [1].seconds = numItem.Value;

		if (SelectedPreset)
			SelectedPreset.ShowEditButton ();

		//CheckDemo (config);
	}

	public void OnRestMinutes (NumItem numItem)
	{
		Timer.Config config = AppRoot.Instance.Timer.Cfg.Clone ();

		AppRoot.Instance.Timer.Cfg.Intervals [2].minutes = numItem.Value;

		if (SelectedPreset)
			SelectedPreset.ShowEditButton ();

		//CheckDemo (config);
	}
	
	public void OnRestSeconds (NumItem numItem)
	{
		Timer.Config config = AppRoot.Instance.Timer.Cfg.Clone ();

		AppRoot.Instance.Timer.Cfg.Intervals [2].seconds = numItem.Value;

		if (SelectedPreset)
			SelectedPreset.ShowEditButton ();

		//CheckDemo (config);
	}

	public void OnRounds (NumItem numItem)
	{
		Timer.Config config = AppRoot.Instance.Timer.Cfg.Clone ();

		AppRoot.Instance.Timer.Cfg.RepetitionCount = numItem.Value;

		if (SelectedPreset)
			SelectedPreset.ShowEditButton ();

		//CheckDemo (config);
	}

	void OnEnable()
	{
		Init();
	
		FillPresets ();

		presetScrollBar.value = 0;
	}

	void OnDisable()
	{
		ShowNumericObjects (false);
	}

	public void OnBack()
	{

		AppRoot.Instance.ShowTimerPage();
	}

	public void OnConfirmPreset(PresetController preset)
	{
		SelectedPreset = null;

		preset.Deselect ();
	}

	public void OnSoundToggle(Toggle toggle)
	{
		if(!InitInprogress)
		{

			AppRoot.Instance.Timer.Cfg.Sound = toggle.isOn;

			if (SelectedPreset)
				SelectedPreset.ShowEditButton ();
		}
	}

	public void DeselectAll()
	{
		for (int i = 0; i < presetContainer.childCount; ++i)
		{
			PresetController presetController = presetContainer.GetChild(i).GetComponent<PresetController>();

			presetController.Deselect();
		}
	}

	public void OnRemovePreset(PresetController preset)
	{
		FillPresets ();

		ReinitPresetContainer ();
	}

	private void CreateNewPreset(string name)
	{
		Timer.Config config = AppRoot.Instance.Timer.CreateNewConfig ();

		config.Name = name;

		AppRoot.Instance.Timer.Presets.Add (config);


//		GameObject presetGo = Instantiate(presetPrefab) as GameObject;
//
//		PresetController presetController = presetGo.transform.GetComponent<PresetController>();
//		
//		presetController.ExtText.SetTextKey(config.Name);
//		presetController.PresetName = config.Name;
//		presetController.name = config.Name;
//		presetController.Removable = true;
//	
//		return presetController;	
	}

	void SelectPreset(PresetController presetController)
	{
		SelectedPreset = presetController;
	
		SelectedPreset.Hightlight ();
	}
	
}
