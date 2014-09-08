using UnityEngine;
using System.Collections;

public class PresetController : MonoBehaviour
{
	public string PresetName { get; set; }

	public core.ui.ExtText ExtText { private set; get; }

	void Awake()
	{
		ExtText = GetComponentInChildren<core.ui.ExtText> ();
	}


	public void OnClick()
	{
		AppRoot.Instance.OnPresetClick (this);
	}
}
