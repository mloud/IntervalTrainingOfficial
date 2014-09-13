using UnityEngine;
using System.Collections;

public class MyDebug : MonoBehaviour {

	bool Enabled { get; set; }


	void Start ()
	{
		Enabled = true;
	}

	void OnGUI()
	{
		if (!enabled)
			return;
		

		if(GUILayout.Button("CaptureScreen"))
		{
			AppRoot.Instance.StartCoroutine(CaptureScreen(this));
		}

		if(GUILayout.Button("RestorePresets"))
		{
			(AppRoot.Instance.Save as trn.CustomSave).RestorePresets();
		}
	}


	private IEnumerator CaptureScreen(MyDebug debug)
	{
		debug.gameObject.SetActive (false);

		yield return new WaitForSeconds(1.0f);

		Application.CaptureScreenshot ("screenshot_" + Time.time +".png");

		yield return new WaitForSeconds(1.0f);

		debug.gameObject.SetActive (true);
	}
}
