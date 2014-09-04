using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InfoController : MonoBehaviour {

	[SerializeField]
	Button btnFb;

	[SerializeField]
	Button btnEmail;



	void Start () {
	
	}
	
	void Update () {
	
	}

	public void OnButtonClick(Button button)
	{
		if (button == btnFb)
		{
			Utils.Instance.OpenFacebookPage();
		}
		else if (button == btnEmail)
		{
			Utils.Instance.SendEmail();
		}
	}

}
