using UnityEngine;
using System.Collections.Generic;

namespace core
{
	namespace dbg
	{
		public class LanguageSwitcher : MonoBehaviour
		{
			void OnGUI()
			{
				for (int i = 0; i < Config.SupportedLanguages.Count; ++i)
				{
					if(GUILayout.Button(Config.SupportedLanguages[i]))
					{
						SwitchToLanguage(Config.SupportedLanguages[i]);
					}
				}
			}

			private void SwitchToLanguage(string language)
			{
				core.TextManager.Instance.LoadTextFile (core.Config.GetLanguageFilename (language));
				
				core.ui.ExtText[] extTexts = GameObject.FindObjectsOfType<core.ui.ExtText> ();

				for (int i = 0; i < extTexts.Length; ++i)
				{
					extTexts[i].ReInit();
				}
			}
		}
	}
}