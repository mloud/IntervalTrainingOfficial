using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace core
{
	public class TextManager
	{
		public static TextManager Instance 
		{ 
			get
			{ 
				if (_instance == null)
				{
					_instance =  new TextManager();
				}

				return _instance;
			}
		}

		private static TextManager _instance;

		private Dictionary<string, string> Texts;
	
		private const string TOKEN_START = "{*";
		private const string TOKEN_END = "*}";


		public bool LoadTextFile(string filename)
		{
			core.dbg.Dbg.Log ("TextManager.LoadTextFile():" + filename);
			bool succ = true;

			try 
			{
				TextAsset textData = Resources.Load("Texts/Lang/" + filename) as TextAsset;

				if (textData != null)
				{
					Texts = new Dictionary<string, string> ();

					StringReader strReader = new StringReader(textData.text);   

					while (true)
					{
						string key = strReader.ReadLine();
						string value = strReader.ReadLine();

						if (key != null && value != null)
						{
							if (Texts.ContainsKey(key))
							{
								core.dbg.Dbg.Log("already exists: " + key);
							}
							else
							{
								Texts.Add(key, value);
								core.dbg.Dbg.Log("adding: " + key + " " + value);
							}
						}
						else if (key != null || value != null)
						{
							succ = false; // unpaired key-value
						}
						else
						{
							// end
							break;
						}
					}
				}
				else
				{
					succ = false;
				}
			}
			catch(IOException ex)
			{
				succ = false;
			}


			return succ;
		}

		public string Get(string key, string tokenName = null, string tokenValue = null)
		{
			string result;
			bool succ = Texts.TryGetValue(key, out result);

			if (succ)
			{
				core.dbg.Dbg.Assert( (tokenName == null && tokenValue == null) || (tokenName != null && tokenValue != null), string.Empty);

				if (tokenName != null && tokenValue != null)
				{
					result = ExpandToken(result, tokenName, tokenValue);
				}

				return result;
			}
			else
			{
				core.dbg.Dbg.Assert(false, "TextManager.Get() " + key + " doesn't exist");
#if DEBUG
				return "?";
#else
				return "";
#endif
			}
		}

		private string ExpandToken(string srcText, string tokenName, string tokenValue)
		{
			core.dbg.Dbg.Log ("TextManager.ExpandToken() in " + srcText + " by " + tokenName + " -> " + tokenValue);

			string tokenNameWithSymbols = TOKEN_START + tokenName + TOKEN_END;

			bool containsToken = srcText.Contains (tokenNameWithSymbols);

			core.dbg.Dbg.Assert(containsToken, "TextManager.ExpandToken() " + srcText + " doesn't containt token " + tokenNameWithSymbols);

			if (containsToken)
			{
				srcText = srcText.Replace(tokenNameWithSymbols, tokenValue);
			}

			core.dbg.Dbg.Log ("TextManager.ExpandToken() result " + srcText);

			return srcText;
		}



	}
}