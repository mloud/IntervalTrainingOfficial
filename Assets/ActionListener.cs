using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ActionListener : MonoBehaviour
{

	public void OnButtoClick(Button button)
	{
		if (button.name == "BtnPlay")
		{
			AppRoot.Instance.Play();
		}
		else if (button.name == "BtnReset")
		{
			//Application.CaptureScreenshot("Screenshot.png");
			AppRoot.Instance.Stop();
		}
		else if (button.name == "BtnPause")
		{
			AppRoot.Instance.PauseTimer();
		}
		else if (button.name == ("BtnPreset"))
		{
			AppRoot.Instance.OnPresetClick(button.GetComponent<PresetController>());
		}
	}

}
