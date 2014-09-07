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
		
		int screenButtonHeight	= Screen.height / 8;
		
		if(GUI.Button(new Rect(0,0,Screen.width,screenButtonHeight), "CaptureScreen"))
		{
			AppRoot.Instance.StartCoroutine(CaptureScreen(this));
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
