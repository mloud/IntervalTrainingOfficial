using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Screener : MonoBehaviour {

	[SerializeField]
	Vector2 designedResolution;




	void Start () 
	{
		List<Text> texts =  FindAllTexts (transform);

		float ratio = (Screen.height / designedResolution.y);

		if (ratio > 1.5f)
		{
			ratio = 1.5f;
		}
		else if(ratio < 0.75)
		{
			ratio = 0.75f;
		}

		foreach(var text in texts)
		{
			text.rectTransform.localScale = new Vector3(ratio, ratio, 1.0f);
		}

	}
	

	private List<Text> FindAllTexts(Transform transform)
	{
		List<Text> texts = new List<Text> ();

		Text text = transform.GetComponent<Text> ();

		if (text != null)
			texts.Add(text);

		for (int i = 0; i < transform.childCount; ++i)
		{
			texts.AddRange(FindAllTexts(transform.GetChild(i)));
		}

		return texts;
	}

	void Update () 
	{
	
	}
}
