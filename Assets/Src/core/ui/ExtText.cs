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


			private Text Text { get; set; }
			private string TextValue { get; set; }

			void Init()
			{
				Text = GetComponent<Text> ();
				
				core.dbg.Dbg.Assert (Text != null, "ExtText.Start(): no Text component found");
			}

			void Start()
			{
				Init ();

				SetTextKey (textKey);
			}

			public void SetTextKey(string key)
			{
				if (Text == null)
				{
					Init ();
				}

				if (key != null)
				{
					textKey = key;

					TextValue = AppRoot.Instance.TextManager.Get (key);

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