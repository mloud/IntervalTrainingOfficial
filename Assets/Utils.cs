using UnityEngine;
using System.Collections;


public class Utils : MonoBehaviour
{

	public static Utils Instance { get; private set; }
	
	void Awake()
	{
		Instance = this;
	}

	public void OpenFacebookPage()
	{
		StartCoroutine (OpenFacebookPageCoroutineInternal());
	}

	public void OpenFullVersionLink()
	{
		Application.OpenURL ("market://details?id=com.MloudWork.IntervalTrainingFull");
	}
	
	private IEnumerator OpenFacebookPageCoroutineInternal()
	{
		Application.OpenURL("fp://www.facebook.com/IntervalTrainer");

		yield return new WaitForSeconds(1);

		if(leftApp)
		{
			leftApp = false;
		}
		else
		{
			Application.OpenURL("https://www.facebook.com/IntervalTrainer");
		}
	}
	
	bool leftApp = false;
	
	void OnApplicationPause()
	{
		leftApp = true;
	}


	public void SendEmail ()
	{
		string email = "gmloud@gmail.com";
		string subject = MyEscapeURL("IntervalTrainer");
		string body = MyEscapeURL("Don't hesitate to ask, report bugs or propose new functions.");
		
		Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
	}
	
	string MyEscapeURL (string url)
	{
		return WWW.EscapeURL(url).Replace("+","%20");
	}




}