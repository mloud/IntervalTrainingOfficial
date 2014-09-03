using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class WorkoutController : MonoBehaviour 
{
	[SerializeField]
	RectTransform pnlExcerciseDb;

	[SerializeField]
	GameObject pnlExcersicePrefab;

	// Use this for initialization
	void Start ()
	{
		List<ExcerciseDb.Excersice> list = Application.Instance.ExcerciseDb.GetExcersices(ExcerciseDb.ExcersiceCategoryType.Trx);

		Vector2 pos = new Vector2(0, pnlExcerciseDb.rect.height * 0.5f);

		for (int i = 0; i <  list.Count; ++i)
		{
			GameObject pnl = Instantiate(pnlExcersicePrefab) as GameObject;
			RectTransform pnlRectTr = pnl.GetComponent<RectTransform>();
			Image image = pnl.transform.GetChild(0).GetComponentInChildren<Image>();
			image.sprite= list[0].sprite;


			pnl.transform.parent = pnlExcerciseDb;

			pnl.transform.localPosition = pos;



			pos -= new Vector2(0, pnlRectTr.rect.height);
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
