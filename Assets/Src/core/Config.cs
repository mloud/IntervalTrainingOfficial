using UnityEngine;
using System.Collections.Generic;

namespace core
{
	public static class Config
	{
		public static List<string> SupportedLanguages { get; private set; }

		private static string LanguageFilename = "text_";
		private static string DefaultLanguage = "english";
		private static string SupportedLanguagesFilename = "Texts/SupportedLanguages";

		public static bool IsLanguageSupported(string language)
		{
			return SupportedLanguages.Find (x => x == language) != null;
		}

		public static string GetLanguageFilename(string language)
		{
			language = language.ToLower ();

			if (IsLanguageSupported(language))
			{
				return LanguageFilename + language;
			}
			else
			{
				return LanguageFilename + DefaultLanguage;
			}
		}

		public static string GetDefaultLanguageFilename()
		{
			return LanguageFilename + DefaultLanguage;
		}

			
			public static void Init()
		{
			LoadSupportedLanguages ();
		}


		private static void LoadSupportedLanguages()
		{
			SupportedLanguages = Utils.ReadLinesFromResourceFile (SupportedLanguagesFilename);

#if DEBUG
			core.dbg.Dbg.Log("Config.LoadSupportedLanguages()" + SupportedLanguages.ToString());
			for (int i = 0; i < SupportedLanguages.Count; ++i)
			{
				core.dbg.Dbg.Log(SupportedLanguages[i]);
			}
#endif
		}

	}
}