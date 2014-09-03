using UnityEngine;
using System.Collections.Generic;

public class FontManager : MonoBehaviour 
{
	public enum FontType
	{
		DigitBig,
		DigitMiddle,
		DigitSmall
	}

	[System.Serializable]
	public class FontBlock
	{
		public FontType Type;
		public Font Font;
	}

	[SerializeField]
	List<FontBlock> fonts;

	public static FontManager Instance { get; private set; }

	void Awake()
	{
		Instance = this;
	}

	public FontBlock GetFont(FontType type)
	{
		return fonts.Find(x=>x.Type == type);
	}

}
