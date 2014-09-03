using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ActionListener : MonoBehaviour
{

	public void OnButtoClick(Button button)
	{
		if (button.name == "BtnPlay")
		{
			Application.Instance.Play();
		}
		else if (button.name == "BtnReset")
		{
			Application.Instance.Stop();
		}
		else if (button.name == "BtnPause")
		{
			Application.Instance.PauseTimer();
		}
		else if (button.name == ("BtnPreset"))
		{
			Application.Instance.OnPresetClick(button.GetComponent<PresetController>());
		}
	}

}
