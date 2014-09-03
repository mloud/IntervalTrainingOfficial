using UnityEngine;
using System.Collections;

public class Splash : MonoBehaviour {

	[SerializeField]
	float duration;


	private float CloseTime { get; set; }

	void Start ()
	{
		CloseTime = Time.time + duration;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (CloseTime > 0 && Time.time > CloseTime)
		{
			UnityEngine.Application.LoadLevelAsync((int)ScendeDef.SceneIndex.Main);
			CloseTime = -1;
		}
	}

	public void AnimationFinished(string animName)
	{
		if (animName == "HeaderAnim")
		{
			//GetComponent<Animator>().Play("Loading");
		}
	}

}
