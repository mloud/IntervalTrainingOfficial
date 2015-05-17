using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace core
{
	namespace ui
	{
		public class ExtText : MonoBehaviour
		{
			[SerializeField]
			string textKey;

			[SerializeField]
			bool toUpper;

			[SerializeField]
			bool SetTextOnStart = true;

			

			public Text Text { get; set; }
			private string TextValue { get; set; }

			void Init()
			{
				Text = GetComponent<Text> ();
				
				core.dbg.Dbg.Assert (Text != null, "ExtText.Start(): no Text component found");
			}

			void Start()
			{
				Init ();

				if (SetTextOnStart)
					SetTextKey (textKey);
			}

		

			public void SetTextKey(string key, string tokenKey = null, string tokenValue = null)
			{
				if (Text == null)
				{
					Init ();
				}

				if (!string.IsNullOrEmpty(key))
				{
					textKey = key;

					if (key.Contains("STR_"))
					{
						TextValue = TextManager.Instance.Get (key, tokenKey, tokenValue);
					}
					else
					{
						TextValue = key;
					}

					if (toUpper)
					{
						TextValue = TextValue.ToUpper();
					}

					Text.text = TextValue;
				}
			}

			public void ReInit()
			{
				SetTextKey (textKey);
			}

		}
	}
}