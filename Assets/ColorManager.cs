using UnityEngine;
using System.Collections.Generic;


namespace Col
{
	public enum ColorType
	{
		WarmUp,
		Work,
		Rest,
		CoolDown
	}

	public class ColorManager : MonoBehaviour 
	{
		[System.Serializable]
		private class ColorDef
		{
			public ColorType Type;
			public Color Color;
		}

		[SerializeField]
		private List<ColorDef> Colors;

		public static ColorManager Instance { get; private set; }

		void Awake()
		{
			Instance = this;
		}

		public Color GetColor(ColorType type)
		{
			return Colors.Find(x=>x.Type == type).Color;
		}
		
	}
}
