using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour 
{
	[SerializeField]
	Scrollbar scrollBar;

	void Awake () 
	{}
	
	void Update () 
	{}

	//0-1
	public void Set(float value)
	{
		scrollBar.size = value;
	}
	
	//0-100
	public void Set(int value)
	{
		Set (value / 100.0f);
	}
}
