using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ExcerciseDb : MonoBehaviour
{
	public enum ExcersiceCategoryType
	{
		Trx = 0,
	}

	[System.Serializable]
	public class Excersice
	{
		public string name;
		public Sprite sprite;
	}


	[System.Serializable]
	public class ExcersiceCategory
	{
		public ExcersiceCategoryType Type;	
		public List<Excersice> ExcerciseList;
	}

	[SerializeField]
	public List<ExcersiceCategory> ExcerciseCategories;


	public List<Excersice> GetExcersices(ExcersiceCategoryType type)
	{
		return ExcerciseCategories.Find(x=>x.Type == type).ExcerciseList ;
	}

}
