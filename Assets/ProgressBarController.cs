using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ProgressBarController : MonoBehaviour 
{

	[SerializeField]
	Image imgBg;

	[SerializeField]
	Image imgFill;

	private Vector3 fillFullScale;


	void Awake()
	{
		fillFullScale = imgFill.rectTransform.localScale;
	}


	void Start () 
	{
	}
	
	void Update () 
	{
	}

	//0-1
	public void Set(float value)
	{
		imgFill.rectTransform.localScale = new Vector3(fillFullScale.x * value, fillFullScale.y, fillFullScale.z);
	}

	//0-100
	public void Set(int value)
	{
		Set (value / 100.0f);
	}

}
