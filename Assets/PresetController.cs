using UnityEngine;
using System.Collections;

public class PresetController : MonoBehaviour
{
	public string PresetName { get; set; }

	void Start () 
	{
	
	}
	
	public void OnClick()
	{
		AppRoot.Instance.OnPresetClick (this);
	}
}
