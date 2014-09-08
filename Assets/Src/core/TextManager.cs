using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace core
{
	public class TextManager
	{
		private Dictionary<string, string> Texts;

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
							Texts.Add(key, value);
							core.dbg.Dbg.Log("adding: " + key + " " + value);
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

		public string Get(string key)
		{
			string result;
			bool succ = Texts.TryGetValue(key, out result);

			if (succ)
			{
				return result;
			}
			else
			{
				core.dbg.Dbg.Log("TextManager.Get() " + key + " doesn't exist");
#if DEBUG
				return "?";
#else
				return "";
#endif
			}
		}
	}
}